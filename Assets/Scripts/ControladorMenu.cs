using UnityEngine;
using UnityEngine.SceneManagement;
public abstract class PantallaBase : MonoBehaviour
{
    public abstract void CambiarEscena(string nombre);
}
public class ControladorMenu : PantallaBase
{    public override void CambiarEscena(string nombre)
    {SceneManager.LoadScene(nombre);
    }
    public void Salir()
    {
        Application.Quit();
    }
}