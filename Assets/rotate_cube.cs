using UnityEngine;

public class rotate_cube : MonoBehaviour
{
    public float velocitaRotazione = 50f;
    public float velocitaFluttuazione = 2f;
    public float altezzaFluttuazione = 0.05f;

    private Vector3 posizioneIniziale;

    void Start()
    {
        posizioneIniziale = transform.localPosition;
    }

    void Update()
    {
        // Ruota l'oggetto sull'asse Y
        transform.Rotate(Vector3.up * velocitaRotazione * Time.deltaTime);

        // Fa oscillare l'oggetto su e giù
        float nuovaY = posizioneIniziale.y + Mathf.Sin(Time.time * velocitaFluttuazione) * altezzaFluttuazione;
        transform.localPosition = new Vector3(posizioneIniziale.x, nuovaY, posizioneIniziale.z);
    }
}