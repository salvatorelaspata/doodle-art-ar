using UnityEngine;

// Movimento continuo delle decorazioni del menu: oscillazione della rotazione e leggera fluttuazione
[RequireComponent(typeof(RectTransform))]
public class DecorazioneOndeggiante : MonoBehaviour
{
    public float ampiezzaRotazione = 6f;
    public float ampiezzaFluttuazione = 10f;
    public float velocita = 1.5f;
    public float sfasamento = 0f;

    private RectTransform rectTransform;
    private Vector2 posizioneIniziale;
    private float rotazioneIniziale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posizioneIniziale = rectTransform.anchoredPosition;
        rotazioneIniziale = rectTransform.localEulerAngles.z;
    }

    void Update()
    {
        float t = Time.time * velocita + sfasamento;

        // Ruota avanti e indietro attorno alla rotazione iniziale
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotazioneIniziale + Mathf.Sin(t) * ampiezzaRotazione);

        // Fa galleggiare l'oggetto su e giù
        rectTransform.anchoredPosition = posizioneIniziale + Vector2.up * (Mathf.Sin(t * 0.8f) * ampiezzaFluttuazione);
    }
}
