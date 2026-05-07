using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject); 
    }

    public void DestroyParent()
    {
        try
        {
            Destroy(gameObject.transform.parent.gameObject);
        } catch {
            Destroy(gameObject.transform);
        }
    }
}
