using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Regia della visita: postazioni davanti alle opere, vista di dettaglio e interfaccia della galleria
public class GalleriaController : MonoBehaviour
{
    public enum ModalitaNavigazione
    {
        Postazioni,
        CamminataLibera // prevista per una fase successiva, non ancora implementata
    }

    [Header("Scena")]
    public CorniceOpera[] opere;
    public Transform ingresso;
    public CameraGalleria cameraGalleria;
    public InputGalleria input;

    [Header("Interfaccia")]
    public Canvas canvas;
    public TMP_Text testoTitolo;
    public TMP_Text testoSottotitolo;
    public CanvasGroup barraNavigazione;
    public PannelloOpera pannello;
    public CanvasGroup avviso;
    public TMP_Text testoAvviso;

    [Header("Inquadratura")]
    public float margineEsplorazione = 0.3f;
    public float margineDettaglio = 0.1f;
    public float riservaAlto = 180f;   // unità del Canvas occupate dai pulsanti in alto
    public float riservaBasso = 270f;  // unità del Canvas occupate dalla barra di navigazione

    public ModalitaNavigazione Modalita { get; private set; } = ModalitaNavigazione.Postazioni;

    private int indice = -1; // -1 = ingresso
    private bool dettaglio;
    private Vector2Int ultimaRisoluzione;
    private Coroutine avvisoInCorso;

    void OnEnable()
    {
        input.Tocco += Tocco;
        input.Trascinamento += Trascinamento;
        input.Swipe += Sposta;
    }

    void OnDisable()
    {
        input.Tocco -= Tocco;
        input.Trascinamento -= Trascinamento;
        input.Swipe -= Sposta;
    }

    void Start()
    {
        ultimaRisoluzione = new Vector2Int(Screen.width, Screen.height);
        avviso.alpha = 0f;
        VaiA(-1, true);
    }

    void Update()
    {
        // Ruotando il telefono cambia l'inquadratura migliore
        var risoluzione = new Vector2Int(Screen.width, Screen.height);
        if (risoluzione != ultimaRisoluzione)
        {
            ultimaRisoluzione = risoluzione;
            AggiornaCamera(false);
        }

        // Durante il dettaglio la barra di navigazione lascia spazio alla scheda
        barraNavigazione.alpha = Mathf.MoveTowards(barraNavigazione.alpha, dettaglio ? 0f : 1f, Time.unscaledDeltaTime * 6f);
        barraNavigazione.interactable = !dettaglio;
        barraNavigazione.blocksRaycasts = !dettaglio;
    }

    public void Successiva()
    {
        Sposta(1);
    }

    public void Precedente()
    {
        Sposta(-1);
    }

    public void ApriDettaglioCorrente()
    {
        if (indice < 0)
        {
            Successiva();
            return;
        }
        dettaglio = true;
        pannello.Mostra(opere[indice].dati);
        AggiornaCamera(false);
    }

    public void ChiudiDettaglio()
    {
        if (!dettaglio)
        {
            return;
        }
        dettaglio = false;
        pannello.Nascondi();
        AggiornaCamera(false);
    }

    public void AttivaCamminataLibera()
    {
        // TODO: camminata libera (movimento e sguardo liberi nella stanza), da implementare
        MostraAvviso("Camminata libera: in arrivo!");
    }

    void Sposta(int passo)
    {
        if (opere.Length == 0)
        {
            return;
        }

        if (dettaglio)
        {
            // Nel dettaglio si sfogliano direttamente le schede delle opere
            indice = (indice + passo + opere.Length) % opere.Length;
            pannello.Mostra(opere[indice].dati);
            AggiornaHUD();
            AggiornaCamera(false);
            return;
        }

        // Ingresso e opere formano un unico giro
        int totale = opere.Length + 1;
        int posizione = (indice + 1 + passo + totale) % totale;
        VaiA(posizione - 1, false);
    }

    void VaiA(int nuovoIndice, bool immediato)
    {
        indice = nuovoIndice;
        AggiornaHUD();
        AggiornaCamera(immediato);
    }

    void AggiornaCamera(bool immediato)
    {
        if (indice < 0)
        {
            cameraGalleria.VaiA(ingresso.position, ingresso.rotation, immediato);
            return;
        }

        float scala = canvas.scaleFactor;
        Rect sicura = Screen.safeArea;
        float alto = 1f - (Screen.height - sicura.yMax + riservaAlto * scala) / Screen.height;

        CorniceOpera cornice = opere[indice];
        if (dettaglio)
        {
            cameraGalleria.InquadraOpera(cornice, cornice.DimensioniTela, pannello.AreaLibera(alto), margineDettaglio, false, immediato);
        }
        else
        {
            float basso = (sicura.yMin + riservaBasso * scala) / Screen.height;
            Rect area = Rect.MinMaxRect(0.03f, basso, 0.97f, alto);
            cameraGalleria.InquadraOpera(cornice, cornice.DimensioniEsterne, area, margineEsplorazione, true, immediato);
        }
    }

    void AggiornaHUD()
    {
        if (indice < 0)
        {
            testoTitolo.text = "Benvenuto!";
            testoSottotitolo.text = "Usa le frecce o tocca un'opera";
            return;
        }
        OperaDati dati = opere[indice].dati;
        testoTitolo.text = dati.titolo;
        testoSottotitolo.text = $"{indice + 1} / {opere.Length} · {dati.autore}";
    }

    void Tocco(Vector2 posizione)
    {
        if (dettaglio)
        {
            ChiudiDettaglio();
            return;
        }

        Ray raggio = cameraGalleria.Telecamera.ScreenPointToRay(posizione);
        if (!Physics.Raycast(raggio, out RaycastHit colpo, 50f))
        {
            return;
        }

        CorniceOpera cornice = colpo.collider.GetComponentInParent<CorniceOpera>();
        int toccata = cornice != null ? Array.IndexOf(opere, cornice) : -1;
        if (toccata < 0)
        {
            return;
        }

        // Toccando l'opera corrente si apre il dettaglio, toccandone un'altra ci si sposta lì
        if (toccata == indice)
        {
            ApriDettaglioCorrente();
        }
        else
        {
            VaiA(toccata, false);
        }
    }

    void Trascinamento(Vector2 delta)
    {
        if (!dettaglio)
        {
            cameraGalleria.Ruota(delta);
        }
    }

    void MostraAvviso(string testo)
    {
        testoAvviso.text = testo;
        if (avvisoInCorso != null)
        {
            StopCoroutine(avvisoInCorso);
        }
        avvisoInCorso = StartCoroutine(AnimaAvviso());
    }

    IEnumerator AnimaAvviso()
    {
        while (avviso.alpha < 1f)
        {
            avviso.alpha = Mathf.MoveTowards(avviso.alpha, 1f, Time.unscaledDeltaTime * 6f);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1.8f);
        while (avviso.alpha > 0f)
        {
            avviso.alpha = Mathf.MoveTowards(avviso.alpha, 0f, Time.unscaledDeltaTime * 3f);
            yield return null;
        }
        avvisoInCorso = null;
    }
}
