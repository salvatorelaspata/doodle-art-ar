using UnityEngine;

// Cornice di un'opera: adatta tela, passe-partout e bordi alle dimensioni reali e mostra l'immagine.
// L'asse Z della cornice punta dentro il muro, quindi la stanza sta dalla parte delle Z negative.
[ExecuteAlways]
public class CorniceOpera : MonoBehaviour
{
    public OperaDati dati;

    [Header("Parti della cornice")]
    public Renderer tela;
    public Transform passepartout;
    public Transform bordoSuperiore;
    public Transform bordoInferiore;
    public Transform bordoSinistro;
    public Transform bordoDestro;
    public BoxCollider areaTocco;

    [Header("Misure (metri)")]
    public float marginePassepartout = 0.1f;
    public float spessoreCornice = 0.08f;
    public float profonditaCornice = 0.07f;

    private const float distanzaTela = 0.035f;
    private static readonly int idTexture = Shader.PropertyToID("_BaseMap");
    private MaterialPropertyBlock blocco;

    // Centro della tela, leggermente staccato dal muro
    public Vector3 Centro => transform.TransformPoint(new Vector3(0f, 0f, -distanzaTela));

    // Direzione verso la stanza
    public Vector3 Normale => -transform.forward;

    public Vector2 DimensioniTela => dati != null
        ? new Vector2(Mathf.Max(0.1f, dati.larghezzaCm / 100f), Mathf.Max(0.1f, dati.altezzaCm / 100f))
        : Vector2.one;

    public Vector2 DimensioniEsterne => DimensioniTela + Vector2.one * 2f * (marginePassepartout + spessoreCornice);

    void OnEnable()
    {
        Aggiorna();
    }

    void Update()
    {
        // In editor la cornice segue subito le modifiche ai dati dell'opera
        if (!Application.isPlaying)
        {
            Aggiorna();
        }
    }

    public void Aggiorna()
    {
        Vector2 dimensioni = DimensioniTela;
        float s = spessoreCornice;
        float p = profonditaCornice;
        float larghezzaInterna = dimensioni.x + 2f * marginePassepartout;
        float altezzaInterna = dimensioni.y + 2f * marginePassepartout;

        if (tela != null)
        {
            Posiziona(tela.transform, new Vector3(0f, 0f, -distanzaTela), new Vector3(dimensioni.x, dimensioni.y, 1f));
            if (dati != null && dati.immagine != null)
            {
                if (blocco == null)
                {
                    blocco = new MaterialPropertyBlock();
                }
                tela.GetPropertyBlock(blocco);
                blocco.SetTexture(idTexture, dati.immagine);
                tela.SetPropertyBlock(blocco);
            }
        }

        Posiziona(passepartout, new Vector3(0f, 0f, -0.015f), new Vector3(larghezzaInterna, altezzaInterna, 0.03f));
        Posiziona(bordoSuperiore, new Vector3(0f, altezzaInterna / 2f + s / 2f, -p / 2f), new Vector3(larghezzaInterna + 2f * s, s, p));
        Posiziona(bordoInferiore, new Vector3(0f, -(altezzaInterna / 2f + s / 2f), -p / 2f), new Vector3(larghezzaInterna + 2f * s, s, p));
        Posiziona(bordoSinistro, new Vector3(-(larghezzaInterna / 2f + s / 2f), 0f, -p / 2f), new Vector3(s, altezzaInterna, p));
        Posiziona(bordoDestro, new Vector3(larghezzaInterna / 2f + s / 2f, 0f, -p / 2f), new Vector3(s, altezzaInterna, p));

        if (areaTocco != null)
        {
            Vector3 centro = new Vector3(0f, 0f, -p / 2f);
            Vector3 misura = new Vector3(larghezzaInterna + 2f * s, altezzaInterna + 2f * s, p);
            if (areaTocco.center != centro) areaTocco.center = centro;
            if (areaTocco.size != misura) areaTocco.size = misura;
        }
    }

    // Assegna solo se cambia, per non sporcare la scena a ogni aggiornamento in editor
    static void Posiziona(Transform parte, Vector3 posizione, Vector3 scala)
    {
        if (parte == null)
        {
            return;
        }
        if (parte.localPosition != posizione) parte.localPosition = posizione;
        if (parte.localScale != scala) parte.localScale = scala;
    }
}
