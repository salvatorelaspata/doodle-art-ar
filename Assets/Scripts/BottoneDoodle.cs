using UnityEngine;
using UnityEngine.EventSystems;

// Effetto "adesivo premuto": alla pressione la faccia della scheda scende sulla sua ombra
public class BottoneDoodle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public RectTransform faccia;
    public Vector2 spostamentoPremuto = new Vector2(14f, -14f);
    public float velocita = 25f;

    private Vector2 posizioneIniziale;
    private bool premuto;

    void Awake()
    {
        if (faccia != null)
        {
            posizioneIniziale = faccia.anchoredPosition;
        }
    }

    void Update()
    {
        if (faccia == null)
        {
            return;
        }

        // Interpolazione indipendente dal frame rate verso la posizione di destinazione
        Vector2 destinazione = premuto ? posizioneIniziale + spostamentoPremuto : posizioneIniziale;
        float t = 1f - Mathf.Exp(-velocita * Time.unscaledDeltaTime);
        faccia.anchoredPosition = Vector2.Lerp(faccia.anchoredPosition, destinazione, t);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        premuto = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        premuto = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        premuto = false;
    }
}
