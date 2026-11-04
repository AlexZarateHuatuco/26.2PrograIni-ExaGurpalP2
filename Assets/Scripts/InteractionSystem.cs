using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private Transform puntoAgarre;
    private ItemKey llaveMano;

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (llaveMano == null)
            {
                Ray ray = new Ray(transform.position, transform.forward);

                if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion))
                {
                    ItemKey llave = hit.collider.GetComponent<ItemKey>();

                    if (llave != null)
                    {
                        llaveMano = llave;
                        llave.Agarrar(puntoAgarre);
                    }
                }
            }
            else
            {
                llaveMano.Soltar();
                llaveMano = null;
            }
        }
    }
}
