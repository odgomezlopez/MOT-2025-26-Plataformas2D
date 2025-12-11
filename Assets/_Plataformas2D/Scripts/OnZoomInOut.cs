using Unity.Cinemachine;
using UnityEngine;

public class OnZoomInOut : MonoBehaviour
{
    [SerializeField] int targetCameraPriority = 50;
    [SerializeField] bool looped = false;
    [SerializeField] CinemachineCamera cinemachineCamera;

    private void Start()
    {
        if (cinemachineCamera == null)
        {
            cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!cinemachineCamera) return;

        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.Priority = targetCameraPriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!cinemachineCamera) return;

        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.Priority = -1;
            if(!looped) gameObject.SetActive(false);
        }
    }
}
