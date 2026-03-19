using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject); 
    }

    public void DestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject); 
    }
}
