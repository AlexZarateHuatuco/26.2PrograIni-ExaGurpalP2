using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int vidaActual = 100;
    [SerializeField] private int vidaMaxima = 100;

    [SerializeField] private float velocidadCaminar = 2f;
    [SerializeField] private float velocidadCorrer = 5f;
    [SerializeField] private float gravedad = -9.81f;
    private float velocidadVertical;
    [SerializeField] private CharacterController controlador;

    protected static event Action<int, int> OnVidaCambiada;

    private void Awake()
    {
        if (controlador == null)
        {
            controlador = GetComponent<CharacterController>();
        }
    }

    private void Start()
    {
        OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
    }
    
    private void Update()
    {
        MovimientoJugador();
    }

    private void MovimientoJugador()
    {
        float x = 0f;
        float y = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed)
            {
                x += 1f;
            }
            if (Keyboard.current.aKey.isPressed)
            {
                x -= 1f;
            }
            if (Keyboard.current.wKey.isPressed)
            {
                y += 1f;
            }
            if (Keyboard.current.sKey.isPressed)
            {
                y -= 1f;
            }
        }

        bool estaMoviendose = x != 0 || y != 0;
        bool presionaShift = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        bool estaCorriendo = presionaShift && estaMoviendose;

        float velocidadActual;
        if (estaCorriendo)
        {
            velocidadActual = velocidadCorrer;
        }
        else
        {
            velocidadActual = velocidadCaminar;
        }

        Vector3 mover = (transform.right * x + transform.forward * y).normalized;

        if (controlador.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        velocidadVertical += gravedad * Time.deltaTime;

        Vector3 movimientoFinal = mover * velocidadActual;
        mover.y = velocidadVertical;
        movimientoFinal.y = velocidadVertical;
        controlador.Move(movimientoFinal * Time.deltaTime);
    }
}
