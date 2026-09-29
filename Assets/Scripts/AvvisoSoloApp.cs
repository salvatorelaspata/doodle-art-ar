using UnityEngine;

// Avviso mostrato sul Web al posto dell'esperienza AR: invita a scaricare l'app dagli store
public class AvvisoSoloApp : MonoBehaviour
{
    public CanvasGroup gruppo;
    public RectTransform scheda;
    public float durata = 0.2f;

    public bool Visibile { get; private set; }

    private float progresso;

    void Start()
    {
        Applica(0f);
    }

    void Update()
    {
        float obiettivo = Visibile ? 1f : 0f;
        if (!Mathf.Approximately(progresso, obiettivo))
        {
            progresso = Mathf.MoveTowards(progresso, obiettivo, Time.unscaledDeltaTime / durata);
            Applica(progresso);
        }
    }

    public void Mostra()
    {
        Visibile = true;
        gruppo.interactable = true;
        gruppo.blocksRaycasts = true;
    }

    public void Nascondi()
    {
        Visibile = false;
        gruppo.interactable = false;
        gruppo.blocksRaycasts = false;
    }

    void Applica(float valore)
    {
        float e = 1f - Mathf.Pow(1f - valore, 3f);
        gruppo.alpha = e;
        scheda.localScale = Vector3.one * Mathf.Lerp(0.85f, 1f, e);
    }
}
