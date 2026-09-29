using UnityEngine;
using UnityEngine.SceneManagement; // Namespace per gestire le scene

public class RouterController : MonoBehaviour
{
    // Usato solo nella build Web, dove l'esperienza AR non è disponibile
    public AvvisoSoloApp avvisoSoloApp;

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
#if UNITY_WEBGL
        // Vuforia non supporta il Web: al posto della scena AR si invita a scaricare l'app
        if (avvisoSoloApp != null)
        {
            avvisoSoloApp.Mostra();
        }
        else
        {
            Debug.LogWarning("Esperienza AR non disponibile sul Web e nessun avviso collegato.");
        }
#else
        Debug.Log("Aprendo l'esperienza AR...");
        SceneManager.LoadScene("03_EsperienzaAR");
        Debug.Log("Esperienza AR caricata.");
#endif
    }

    // Funzione opzionale per chiudere l'app
    public void EsciDallApp()
    {
        Debug.Log("Uscita dall'app in corso...");
        Application.Quit();
        Debug.Log("App chiusa.");
    }
}