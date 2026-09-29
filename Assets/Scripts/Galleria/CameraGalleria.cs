using UnityEngine;

// Camera della galleria: scivola tra le postazioni, inquadra le opere e ruota con il trascinamento
[RequireComponent(typeof(Camera))]
public class CameraGalleria : MonoBehaviour
{
    public float velocita = 3.5f;
    public float campoVisivoVerticale = 60f;
    public float campoVisivoOrizzontaleMinimo = 40f;
    public float gradiPerSchermo = 90f;
    public float limiteOrizzontale = 40f;
    public float limiteVerticale = 20f;
    public float distanzaMinima = 1.5f;
    public float distanzaMassima = 5f;

    private Camera telecamera;
    private Vector3 posizioneTarget;
    private Quaternion rotazioneTarget;
    private Vector2 sguardo;

    public Camera Telecamera => telecamera != null ? telecamera : (telecamera = GetComponent<Camera>());

    void Awake()
    {
        posizioneTarget = transform.position;
        rotazioneTarget = transform.rotation;
    }

    void LateUpdate()
    {
        AggiornaCampoVisivo();
        float t = 1f - Mathf.Exp(-velocita * Time.unscaledDeltaTime);
        Quaternion obiettivo = rotazioneTarget * Quaternion.Euler(sguardo.y, sguardo.x, 0f);
        transform.position = Vector3.Lerp(transform.position, posizioneTarget, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, obiettivo, t);
    }

    public void VaiA(Vector3 posizione, Quaternion rotazione, bool immediato)
    {
        posizioneTarget = posizione;
        rotazioneTarget = rotazione;
        sguardo = Vector2.zero;
        if (immediato)
        {
            transform.SetPositionAndRotation(posizione, rotazione);
        }
    }

    // Il contenuto segue il dito: trascinando verso destra la vista gira a sinistra
    public void Ruota(Vector2 deltaPixel)
    {
        float gradiPerPixel = gradiPerSchermo / Mathf.Max(1, Screen.height);
        sguardo.x = Mathf.Clamp(sguardo.x - deltaPixel.x * gradiPerPixel, -limiteOrizzontale, limiteOrizzontale);
        sguardo.y = Mathf.Clamp(sguardo.y + deltaPixel.y * gradiPerPixel, -limiteVerticale, limiteVerticale);
    }

    // Con lo schermo verticale si allarga il campo visivo, per non guardare la stanza "dal buco della serratura"
    public void AggiornaCampoVisivo()
    {
        float mezzoOrizzontale = campoVisivoOrizzontaleMinimo * 0.5f * Mathf.Deg2Rad;
        float verticaleNecessario = 2f * Mathf.Atan(Mathf.Tan(mezzoOrizzontale) / Telecamera.aspect) * Mathf.Rad2Deg;
        Telecamera.fieldOfView = Mathf.Max(campoVisivoVerticale, verticaleNecessario);
    }

    // Porta l'opera, frontalmente, dentro la porzione di schermo indicata (coordinate viewport 0-1)
    public void InquadraOpera(CorniceOpera cornice, Vector2 dimensioni, Rect area, float margine, bool limitaDistanza, bool immediato)
    {
        AggiornaCampoVisivo();
        float tanV = Mathf.Tan(Telecamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float tanH = tanV * Telecamera.aspect;
        Vector2 ingombro = dimensioni * (1f + margine);

        float distanza = Mathf.Max(ingombro.y / (2f * area.height * tanV), ingombro.x / (2f * area.width * tanH));
        if (limitaDistanza)
        {
            distanza = Mathf.Clamp(distanza, distanzaMinima, distanzaMassima);
        }

        Vector3 avanti = -cornice.Normale;
        Vector3 destra = Vector3.Cross(Vector3.up, avanti);
        float spostamentoX = (area.center.x - 0.5f) * 2f * distanza * tanH;
        float spostamentoY = (area.center.y - 0.5f) * 2f * distanza * tanV;

        Vector3 posizione = cornice.Centro + cornice.Normale * distanza - destra * spostamentoX - Vector3.up * spostamentoY;
        VaiA(posizione, Quaternion.LookRotation(avanti, Vector3.up), immediato);
    }
}
