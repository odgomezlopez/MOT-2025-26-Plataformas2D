using UnityEngine;


[DisallowMultipleComponent]
[DefaultExecutionOrder(10000)] // after Cinemachine most of the time
public class Parallax : MonoBehaviour
{
    [Tooltip("Real Camera transform (with CinemachineBrain). Leave empty to use Camera.main.")]
    [SerializeField] private Transform cameraTransform;

    [Header("Parallax Factors (0 = fixed, 1 = follow camera)")]
    [Range(-2f, 2f)][SerializeField] private float parallaxX = 0.3f;
    [Range(-2f, 2f)][SerializeField] private float parallaxY = 0.1f;

    [Header("Axis")]
    [SerializeField] private bool affectX = true;
    [SerializeField] private bool affectY = true;

    private Vector3 _layerStartPos;
    private Vector3 _cameraStartPos;

    private void OnEnable()
    {
        if (!cameraTransform)
        {
            var cam = Camera.main;
            cameraTransform = cam ? cam.transform : null;
        }

        Recenter();
    }

    private void LateUpdate()
    {
        if (!cameraTransform) return;

        Vector3 camDelta = cameraTransform.position - _cameraStartPos;

        Vector3 p = _layerStartPos;
        if (affectX) p.x += camDelta.x * parallaxX;
        if (affectY) p.y += camDelta.y * parallaxY;

        transform.position = p;
    }

    /// Call after teleports / hard cuts.
    public void Recenter()
    {
        _layerStartPos = transform.position;
        if (cameraTransform) _cameraStartPos = cameraTransform.position;
    }
}
