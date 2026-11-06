using UnityEngine;
using TMPro;

public class ControlHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoLlaves;
    [SerializeField] private TextMeshProUGUI textoVida;

    private void OnEnable()
    {
        PlayerController.OnVidaCambiada += ActualizarHUDVida;
    }

    private void OnDisable()
    {
        PlayerController.OnVidaCambiada -= ActualizarHUDVida;
    }

    private void Update()
    {
        if (GestorJuego.instance != null && textoLlaves != null)
        {
            textoLlaves.text = "Llaves: " + GestorJuego.instance.llaves + "/4";
        }
    }

    private void ActualizarHUDVida(int vidaActual, int vidaMaxima)
    {
        if (textoVida != null)
        {
            textoVida.text = "Vida: " + vidaActual + " / " + vidaMaxima;
        }
    }
}
