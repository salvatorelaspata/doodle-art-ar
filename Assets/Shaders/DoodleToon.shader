// Shader cartoon per la galleria: colori pieni, due toni di luce e tratti di inchiostro.
// Non usa luci in tempo reale né lightmap: leggero anche su dispositivi mobili.
Shader "Doodle/Toon"
{
    Properties
    {
        [MainColor] _BaseColor ("Colore", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap ("Texture", 2D) = "white" {}
        _ScalaMondo ("Metri per ripetizione (0 = UV della mesh)", Float) = 0
        _ColoreOmbra ("Colore lato in ombra", Color) = (0.85, 0.82, 0.92, 1)
        _DirezioneLuce ("Direzione luce", Vector) = (0.4, 0.8, -0.45, 0)
        _SogliaOmbra ("Soglia ombra", Range(-1, 1)) = 0
        _ColoreInchiostro ("Colore inchiostro", Color) = (0.08, 0.08, 0.08, 1)
        _SpessoreBordi ("Bordi delle facce (pixel)", Range(0, 8)) = 2.5
        _SogliaContorno ("Contorno oggetti curvi", Range(0, 1)) = 0
        _Cutoff ("Soglia trasparenza", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
            float _ScalaMondo;
            half4 _ColoreOmbra;
            float4 _DirezioneLuce;
            half _SogliaOmbra;
            half4 _ColoreInchiostro;
            half _SpessoreBordi;
            half _SogliaContorno;
            half _Cutoff;
        CBUFFER_END

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float3 positionWS : TEXCOORD2;
        };

        Varyings vert(Attributes input)
        {
            Varyings output;
            VertexPositionInputs posizione = GetVertexPositionInputs(input.positionOS.xyz);
            output.positionCS = posizione.positionCS;
            output.positionWS = posizione.positionWS;
            output.normalWS = TransformObjectToWorldNormal(input.normalOS);
            output.uv = input.uv;
            return output;
        }

        // UV della mesh oppure proiezione dal mondo, per avere la stessa densità su muri e pavimento
        float2 CoordinateTexture(Varyings input, float3 n)
        {
            if (_ScalaMondo <= 0)
            {
                return input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
            }
            float3 a = abs(n);
            float2 uvMondo = (a.y > a.x && a.y > a.z) ? input.positionWS.xz : ((a.x > a.z) ? input.positionWS.zy : input.positionWS.xy);
            return uvMondo / _ScalaMondo;
        }

        half4 Campiona(Varyings input, float3 n)
        {
            half4 colore = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, CoordinateTexture(input, n)) * _BaseColor;
            clip(colore.a - _Cutoff);
            return colore;
        }
        ENDHLSL

        Pass
        {
            Name "Doodle"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(Varyings input) : SV_Target
            {
                float3 n = normalize(input.normalWS);
                half3 colore = Campiona(input, n).rgb;

                // Luce a due toni: le facce rivolte dall'altra parte prendono il colore d'ombra
                float luce = dot(n, normalize(_DirezioneLuce.xyz));
                half ombra = 1 - smoothstep(_SogliaOmbra - 0.02, _SogliaOmbra + 0.02, luce);
                colore = lerp(colore, colore * _ColoreOmbra.rgb, ombra);

                // Inchiostro lungo i bordi UV: ogni faccia di un cubo va da 0 a 1, quindi si disegnano gli spigoli
                float2 distanza = min(input.uv, 1 - input.uv) / max(fwidth(input.uv), 1e-5);
                half bordo = (1 - smoothstep(_SpessoreBordi - 0.75, _SpessoreBordi + 0.75, min(distanza.x, distanza.y)))
                    * step(0.01, _SpessoreBordi);

                // Contorno per gli oggetti curvi, dove la superficie è di taglio rispetto alla camera
                float3 vista = GetWorldSpaceNormalizeViewDir(input.positionWS);
                half taglio = 1 - saturate(dot(n, vista));
                half contorno = step(0.001, _SogliaContorno) * smoothstep(_SogliaContorno, _SogliaContorno + 0.05, taglio);

                colore = lerp(colore, _ColoreInchiostro.rgb, saturate(max(bordo, contorno)));
                return half4(colore, 1);
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragProfondita

            half4 fragProfondita(Varyings input) : SV_Target
            {
                Campiona(input, normalize(input.normalWS));
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragNormali

            half4 fragNormali(Varyings input) : SV_Target
            {
                float3 n = normalize(input.normalWS);
                Campiona(input, n);
                return half4(n, 0);
            }
            ENDHLSL
        }
    }
}
