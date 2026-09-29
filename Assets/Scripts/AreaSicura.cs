using UnityEngine;

// Adatta il RectTransform all'area sicura dello schermo (notch, angoli arrotondati, barre di sistema)
[RequireComponent(typeof(RectTransform))]
public class AreaSicura : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect ultimaArea;
    private Vector2Int ultimaRisoluzione;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Aggiorna();
    }

    void Update()
    {
        // L'area sicura cambia quando il dispositivo ruota
        Aggiorna();
    }

    void Aggiorna()
    {
        Rect area = Screen.safeArea;
        Vector2Int risoluzione = new Vector2Int(Screen.width, Screen.height);

        if (area == ultimaArea && risoluzione == ultimaRisoluzione)
        {
            return;
        }
        if (risoluzione.x <= 0 || risoluzione.y <= 0)
        {
            return;
        }

        ultimaArea = area;
        ultimaRisoluzione = risoluzione;

        rectTransform.anchorMin = new Vector2(area.xMin / risoluzione.x, area.yMin / risoluzione.y);
        rectTransform.anchorMax = new Vector2(area.xMax / risoluzione.x, area.yMax / risoluzione.y);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
