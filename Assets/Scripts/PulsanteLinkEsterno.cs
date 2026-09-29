using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

// Apre un indirizzo web al tocco. Nel browser la nuova scheda si apre al rilascio del dito o del
// mouse, l'unico momento in cui i browser lo consentono (vedi Plugins/WebGL/LinkEsterni.jslib).
public class PulsanteLinkEsterno : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("Lasciare vuoto per usare la pagina di download dell'app (LinkApp.PaginaDownload)")]
    public string indirizzo;

    private string Destinazione => string.IsNullOrEmpty(indirizzo) ? LinkApp.PaginaDownload : indirizzo;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void PreparaApriLink(string indirizzo);

    [DllImport("__Internal")]
    private static extern void AnnullaApriLink();

    public void OnPointerDown(PointerEventData eventData)
    {
        PreparaApriLink(Destinazione);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Il dito è uscito dal pulsante prima del rilascio: niente link
        AnnullaApriLink();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
#else
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(Destinazione);
    }
#endif
}
