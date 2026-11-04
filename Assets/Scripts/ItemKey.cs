using UnityEngine;

public class ItemKey : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private bool llaveAgarrado = false;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void Interactuar()
    {

    }

    public void Agarrar(Transform puntoAgarre)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        transform.SetParent(puntoAgarre);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (!llaveAgarrado && GestorJuego.instance != null)
        {
            llaveAgarrado = true;
            GestorJuego.instance.llaves++;
            Debug.Log("LLave en la mano. Total: " + GestorJuego.instance.llaves);
        }
    }

    public void Soltar()
    {
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
