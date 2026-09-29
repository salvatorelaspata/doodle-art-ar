using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Riconosce tocco, trascinamento e swipe con mouse o touch, ignorando i gesti che partono sulla UI
public class InputGalleria : MonoBehaviour
{
    public float sogliaTrascinamento = 12f;
    public float durataMassimaTocco = 0.35f;
    public float durataMassimaSwipe = 0.4f;
    public float lunghezzaMinimaSwipe = 0.18f; // frazione della larghezza dello schermo

    public event Action<Vector2> Tocco;
    public event Action<Vector2> Trascinamento;
    public event Action<int> Swipe; // +1 opera successiva, -1 precedente

    private bool premuto;
    private bool trascina;
    private bool sopraUI;
    private Vector2 inizio;
    private Vector2 ultima;
    private float tempoInizio;
    private readonly List<RaycastResult> risultatiUI = new List<RaycastResult>();

    void Update()
    {
        Pointer puntatore = Pointer.current;
        if (puntatore == null)
        {
            return;
        }

        Vector2 posizione = puntatore.position.ReadValue();

        if (puntatore.press.wasPressedThisFrame)
        {
            premuto = true;
            trascina = false;
            sopraUI = SopraUI(posizione);
            inizio = ultima = posizione;
            tempoInizio = Time.unscaledTime;
        }
        else if (premuto && puntatore.press.isPressed)
        {
            if (!sopraUI)
            {
                if (!trascina && (posizione - inizio).magnitude > sogliaTrascinamento)
                {
                    trascina = true;
                }
                if (trascina)
                {
                    Trascinamento?.Invoke(posizione - ultima);
                }
            }
            ultima = posizione;
        }

        if (premuto && puntatore.press.wasReleasedThisFrame)
        {
            premuto = false;
            if (sopraUI)
            {
                return;
            }

            float durata = Time.unscaledTime - tempoInizio;
            Vector2 totale = posizione - inizio;
            if (!trascina && durata <= durataMassimaTocco)
            {
                Tocco?.Invoke(posizione);
            }
            else if (trascina && durata <= durataMassimaSwipe
                     && Mathf.Abs(totale.x) > Screen.width * lunghezzaMinimaSwipe
                     && Mathf.Abs(totale.x) > Mathf.Abs(totale.y) * 1.5f)
            {
                // Dito verso sinistra = opera successiva, come sfogliare un album
                Swipe?.Invoke(totale.x < 0f ? 1 : -1);
            }
        }
    }

    bool SopraUI(Vector2 posizione)
    {
        if (EventSystem.current == null)
        {
            return false;
        }
        var dati = new PointerEventData(EventSystem.current) { position = posizione };
        risultatiUI.Clear();
        EventSystem.current.RaycastAll(dati, risultatiUI);
        return risultatiUI.Count > 0;
    }
}
