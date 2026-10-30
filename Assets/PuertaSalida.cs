using UnityEngine;
using System;
using TMPro;
public interface IInteractuable
{
    void Interactuar();
}
public class PuertaSalida : MonoBehaviour, IInteractuable
{
    public static event Action alGanar;
    public TMP_Text textoFlotante;
    Func<int, bool> tieneLlaves = (cantidad) => cantidad >= 4;
    public void Interactuar()
    {
        int misLlaves = GestorJuego.instance.llaves;
        if (tieneLlaves(misLlaves))
        {
            if (alGanar != null) alGanar();
        }
        else
        {
            if (textoFlotante != null)
            {
                textoFlotante.gameObject.SetActive(true);
                textoFlotante.text = "Cerrado (" + misLlaves + "/4 llaves)";
            }
        }
    }
    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            Interactuar();
        }
    }
    void OnTriggerExit(Collider otro)
    {
        if (otro.CompareTag("Player") && textoFlotante != null)
        {
            textoFlotante.gameObject.SetActive(false);
        }
    }
}