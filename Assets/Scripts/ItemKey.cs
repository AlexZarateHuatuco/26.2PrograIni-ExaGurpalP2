using UnityEngine;

public class ItemKey : MonoBehaviour, IInteractuable
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

        if (!llaveAgarrado && GestorJuego.instance != null)
        {
            llaveAgarrado = true;
            GestorJuego.instance.llaves++;
        }

        Destroy(gameObject);
    }
}
