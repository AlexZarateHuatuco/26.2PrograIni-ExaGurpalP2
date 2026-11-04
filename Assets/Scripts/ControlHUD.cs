using UnityEngine;
using TMPro;

public class ControlHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoLlaves;

    private void Update()
    {
        if (GestorJuego.instance != null && textoLlaves != null)
        {
            textoLlaves.text = "Llaves: " + GestorJuego.instance.llaves + "/4";
        }
    }
}
