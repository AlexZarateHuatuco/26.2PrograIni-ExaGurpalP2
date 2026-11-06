using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cuerpoJugador;
    [SerializeField] private float sensibilidadMouse = 0.1f;
    private float rotacionX = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null || cuerpoJugador == null)
        {
            return;
        }

        Vector2 movimientoMouse = Mouse.current.delta.ReadValue();

        float mouseX = movimientoMouse.x * sensibilidadMouse;
        float mouseY = movimientoMouse.y * sensibilidadMouse;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        cuerpoJugador.Rotate(Vector3.up * mouseX);
    }
}
