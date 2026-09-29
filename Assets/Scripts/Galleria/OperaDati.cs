using UnityEngine;

// Scheda di un'opera esposta: testi mostrati nella galleria e dimensioni reali della tela
[CreateAssetMenu(fileName = "NuovaOpera", menuName = "Doodle Art/Opera")]
public class OperaDati : ScriptableObject
{
    public string titolo;
    public string autore;
    public string anno;
    [TextArea(3, 8)]
    public string descrizione;
    public Texture2D immagine;
    public float larghezzaCm = 100f;
    public float altezzaCm = 100f;
}
