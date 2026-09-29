using TMPro;
using UnityEngine;

// Scheda con i dettagli dell'opera: in basso con lo schermo verticale, a destra con quello orizzontale
public class PannelloOpera : MonoBehaviour
{
    public RectTransform scheda;   // posizione finale della scheda
    public RectTransform animato;  // contenuto che entra ed esce scorrendo
    public CanvasGroup gruppo;
    public TMP_Text testoTitolo;
    public TMP_Text testoDettagli;
    public TMP_Text testoDescrizione;

    public float margine = 40f;
    public float altezzaVerticale = 700f;
    public float larghezzaOrizzontale = 940f;
    public float durata = 0.25f;

    public bool Visibile { get; private set; }

    private bool orizzontale;
    private bool layoutPronto;
    private float progresso;

    void Start()
    {
        AggiornaLayout(true);
        Applica(0f);
    }

    void Update()
    {
        AggiornaLayout(false);
        float obiettivo = Visibile ? 1f : 0f;
        if (!Mathf.Approximately(progresso, obiettivo))
        {
            progresso = Mathf.MoveTowards(progresso, obiettivo, Time.unscaledDeltaTime / durata);
            Applica(progresso);
        }
    }

    public void Mostra(OperaDati dati)
    {
        testoTitolo.text = dati.titolo;
        testoDettagli.text = $"{dati.autore} · {dati.anno} · {dati.larghezzaCm:0} × {dati.altezzaCm:0} cm";
        testoDescrizione.text = dati.descrizione;
        Visibile = true;
        gruppo.interactable = true;
        gruppo.blocksRaycasts = true;
        AggiornaLayout(true);
    }

    public void Nascondi()
    {
        Visibile = false;
        gruppo.interactable = false;
        gruppo.blocksRaycasts = false;
    }

    // Porzione di schermo (coordinate viewport) lasciata libera dalla scheda, sotto il limite superiore indicato
    public Rect AreaLibera(float limiteAlto)
    {
        AggiornaLayout(false);
        Vector3[] angoli = new Vector3[4];
        scheda.GetWorldCorners(angoli); // con un Canvas in overlay sono coordinate schermo
        if (orizzontale)
        {
            float destra = angoli[0].x / Screen.width;
            return Rect.MinMaxRect(0.02f, 0.04f, destra - 0.02f, limiteAlto);
        }
        float basso = angoli[1].y / Screen.height;
        return Rect.MinMaxRect(0.04f, basso + 0.02f, 0.96f, limiteAlto);
    }

    void AggiornaLayout(bool forza)
    {
        bool nuovoOrizzontale = Screen.width > Screen.height;
        if (!forza && layoutPronto && nuovoOrizzontale == orizzontale)
        {
            return;
        }
        orizzontale = nuovoOrizzontale;
        layoutPronto = true;

        if (orizzontale)
        {
            scheda.anchorMin = new Vector2(1f, 0f);
            scheda.anchorMax = new Vector2(1f, 1f);
            scheda.pivot = new Vector2(1f, 0.5f);
            scheda.anchoredPosition = new Vector2(-margine, 0f);
            scheda.sizeDelta = new Vector2(larghezzaOrizzontale, -2f * margine);
        }
        else
        {
            scheda.anchorMin = new Vector2(0f, 0f);
            scheda.anchorMax = new Vector2(1f, 0f);
            scheda.pivot = new Vector2(0.5f, 0f);
            scheda.anchoredPosition = new Vector2(0f, margine);
            scheda.sizeDelta = new Vector2(-2f * margine, altezzaVerticale);
        }
        Applica(progresso);
    }

    void Applica(float valore)
    {
        float e = 1f - Mathf.Pow(1f - valore, 3f);
        gruppo.alpha = e;
        Vector2 fuori = orizzontale
            ? new Vector2(scheda.rect.width + margine, 0f)
            : new Vector2(0f, -(scheda.rect.height + margine));
        animato.anchoredPosition = Vector2.Lerp(fuori, Vector2.zero, e);
    }
}
