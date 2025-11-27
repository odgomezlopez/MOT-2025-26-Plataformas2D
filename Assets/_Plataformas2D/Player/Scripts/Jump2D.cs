using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump2D : MonoBehaviour
{
    #region Parameters
    [Header("Optional")]
    public UnityEvent OnJump;


    //Compponents
    StatsComponent statsComponent;
    private Rigidbody2D _rb;
    private IGrounded2D _grounded;

    private float _lastJumpPressed = -999f;
    #endregion
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grounded = GetComponentInChildren<IGrounded2D>();
        statsComponent = GetComponent<StatsComponent>();
    }

    public void Jump(InputAction.CallbackContext context = default)
    {
        _lastJumpPressed = Time.time;
    }

    public void Jump(bool triggered)
    {
        if(triggered) _lastJumpPressed = Time.time;
    }

    private void FixedUpdate()
    {
        if (_grounded == null) return;

        // Jump if: recently pressed AND currently grounded (your IGrounded2D can include coyote)
        bool pressedRecently = (Time.time - _lastJumpPressed) <= statsComponent.stats.jumpBuffer;
        
        
        if (pressedRecently && (_grounded.IsGrounded))
        {
            DoJump();
            _lastJumpPressed = -999f; // consume buffer
        }
    }

    private void DoJump()
    {
        // Set vertical velocity directly for consistent jumps
        _rb.linearVelocityY = statsComponent.stats.jumpForce;
        //_rb.AddForce(Vector2.up* jumpVelocity, ForceMode2D.Impulse);
        OnJump?.Invoke();
    }
}
