using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump2D : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Jump")]
    [Tooltip("Upward velocity set on jump. Keep it consistent across mass/gravity.")]
    [SerializeField] private float jumpVelocity = 12f;

    [Tooltip("Allows pressing slightly before landing (seconds).")]
    [SerializeField, Range(0f, 0.2f)] private float jumpBuffer = 0.08f;

    [Header("Optional")]
    [SerializeField] private string animatorJumpTrigger = "Jump";
    public UnityEvent OnJump;


    //Compponents
    private Rigidbody2D _rb;
    private IGrounded2D _grounded;

    private float _lastJumpPressed = -999f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grounded = GetComponentInChildren<IGrounded2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey))
            _lastJumpPressed = Time.time;
    }

    private void FixedUpdate()
    {
        if (_grounded == null) return;

        // Jump if: recently pressed AND currently grounded (your IGrounded2D can include coyote)
        bool pressedRecently = (Time.time - _lastJumpPressed) <= jumpBuffer;
        if (pressedRecently && _grounded.IsGrounded)
        {
            DoJump();
            _lastJumpPressed = -999f; // consume buffer
        }
    }

    private void DoJump()
    {
        // Set vertical velocity directly for consistent jumps
        _rb.linearVelocityY = jumpVelocity;

        OnJump?.Invoke();
    }
}
