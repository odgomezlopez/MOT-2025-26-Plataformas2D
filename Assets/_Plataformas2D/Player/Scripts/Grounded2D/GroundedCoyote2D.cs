using UnityEngine;

[DisallowMultipleComponent]
public class GroundedCoyote2D : MonoBehaviour, IGrounded2D
{
    [Header("Rect Ground Check (local space)")]
    [SerializeField] private Vector2 size = new Vector2(0.45f, 0.08f);
    [SerializeField] private Vector2 offset = new Vector2(0f, -0.10f);
    [SerializeField, Range(0f, 0.1f)] private float edgePadding = 0.02f;
    [SerializeField] private LayerMask groundMask;

    [Header("Coyote Time")]
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.1f;

    public bool IsGroundedRaw { get; private set; }
    public bool IsGrounded { get; private set; }

    private float _lastGroundedTime;

    private void FixedUpdate()
    {
        // world-space rect center from local offset
        Vector2 center = (Vector2)transform.TransformPoint(offset);
        Vector2 padded = new Vector2(size.x + edgePadding * 2f, size.y);
        float angleZ = transform.eulerAngles.z;

        IsGroundedRaw = Physics2D.OverlapBox(center, padded, angleZ, groundMask) != null;

        if (IsGroundedRaw) _lastGroundedTime = Time.time;
        IsGrounded = IsGroundedRaw || (Time.time - _lastGroundedTime <= coyoteTime);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = transform.TransformPoint((Vector3)offset);
        Vector2 padded = new Vector2(size.x + edgePadding * 2f, size.y);

        var prev = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(Vector3.zero, (Vector3)padded);
        Gizmos.matrix = prev;
    }
}
