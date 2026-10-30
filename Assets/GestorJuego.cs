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