using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private float distanciaInteraccion = 3f;
    
    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion))
            {
                IInteractuable interactuable = hit.collider.GetComponent<IInteractuable>();

                if (interactuable != null)
                {
                    interactuable.Interactuar();
                }

                ItemKey llave = hit.collider.GetComponent<ItemKey>();

                if (llave != null)
                {
                    llave.Agarrar(null);
                }
            }
        }
    }
}
