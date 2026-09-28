using UnityEngine;
using UnityEngine.SceneManagement; // Namespace per gestire le scene

public class RouterController : MonoBehaviour
{
    // Carica la Scena del Menu Principale (Indice 0 o Nome)
    public void ApriMenuPrincipale()
    {
        Debug.Log("Aprendo il menu principale...");
        SceneManager.LoadScene("01_MenuPrincipale");
        Debug.Log("Menu principale caricato.");
    }

    // Carica la Scena della Stanza Virtuale 3D
    public void ApriStanzaVirtuale()
    {
        Debug.Log("Aprendo la stanza virtuale 3D...");
        SceneManager.LoadScene("02_StanzaVirtuale3D");
        Debug.Log("Stanza virtuale 3D caricata.");
    }

    // Carica la Scena dell'Esperienza AR con Vuforia
    public void ApriEsperienzaAR()
    {
        Debug.Log("Aprendo l'esperienza AR...");
        SceneManager.LoadScene("03_EsperienzaAR");
        Debug.Log("Esperienza AR caricata.");
    }

    // Funzione opzionale per chiudere l'app
    public void EsciDallApp()
    {
        Debug.Log("Uscita dall'app in corso...");
        Application.Quit();
        Debug.Log("App chiusa.");
    }
}