using UnityEngine;

public class PlatformRider2D : MonoBehaviour
{
    //Si has escalado el sprite y collider la plataforma, poner aquí a una referencia a un padre sin escalar. Mejora el comportamiento.
    [SerializeField] Transform parent;

    private void Awake()
    {
        if(parent == null) parent = transform;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null) collision.gameObject.transform.SetParent(parent, worldPositionStays: true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null) collision.gameObject.transform.SetParent(null, worldPositionStays: true);
    }
}
