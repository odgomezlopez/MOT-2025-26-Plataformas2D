public interface IGrounded2D
{
    bool IsGrounded { get; }      // with coyote time
    bool IsGroundedRaw { get; }   // instant contact (no coyote)
}