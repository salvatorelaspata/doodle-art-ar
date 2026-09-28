using UnityEngine;
using UnityEngine.InputSystem; // Importante per il nuovo Input System

public class interazione_cube : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
        if (arCamera == null)
        {
            arCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        // Controlla se l'utente ha premuto il tasto sinistro del mouse o ha toccato lo schermo
        bool inputRilevato = false;
        Vector2 posizioneInput = Vector2.zero;

        // Rilevamento da Touchscreen (Mobile)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            inputRilevato = true;
            posizioneInput = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        // Rilevamento da Mouse (Mac / PC)
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            inputRilevato = true;
            posizioneInput = Mouse.current.position.ReadValue();
        }

        // Se è stato rilevato un clic/tocco, lancia il Raycast
        if (inputRilevato && arCamera != null)
        {
            Ray ray = arCamera.ScreenPointToRay(posizioneInput);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Debug.Log("Cubo toccato con successo!");
                    EseguiEffetto();
                }
            }
        }
    }

    void EseguiEffetto()
    {
        // Cambia colore al materiale
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Random.ColorHSV();
        }

        // Ingrandisci leggermente l'oggetto
        transform.localScale *= 1.1f;
    }
}