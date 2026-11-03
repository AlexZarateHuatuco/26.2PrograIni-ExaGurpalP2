using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int vidaActual = 100;
    [SerializeField] private int vidaMaxima = 100;

    [SerializeField] private float velocidadCaminar = 4f;
    [SerializeField] private float gravedad = -9.81f;
    private float velocidadVertical;
    [SerializeField] private CharacterController controlador;

    private void Awake()
    {
        if (controlador == null)
        {
            controlador = GetComponent<CharacterController>();
        }
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

        Vector3 mover = (transform.right * x + transform.forward * y).normalized;

        if (controlador.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        velocidadVertical += gravedad * Time.deltaTime;
        mover.y = velocidadVertical;
        controlador.Move(mover * velocidadCaminar * Time.deltaTime);
    }
}
