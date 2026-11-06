using UnityEngine;
using UnityEngine.SceneManagement;
public class GestorJuego : MonoBehaviour
{
    public static GestorJuego instance;
    public int llaves = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PuertaSalida.alGanar += Ganar;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Perder();
        }
    }
    void OnDestroy()
    {
        PuertaSalida.alGanar -= Ganar;
    }
    public void Ganar()
    {
        SceneManager.LoadScene("Victoria");
    }
    public void Perder()
    {
        SceneManager.LoadScene("Derrota");
    }
}