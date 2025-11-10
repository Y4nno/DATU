using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStateList))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float jumpForce = 45f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int jumpBufferFrames = 10;

    private Rigidbody2D rb;
    private PlayerStateList state;
    private Animator anim;

    private float gravity;
    private float scaleX;

    private float coyoteCounter;
    private int jumpBufferCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerStateList>();
        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
        scaleX = transform.localScale.x;
    }

    public void HandleMovement(PlayerInputHandler input)
    {
        Flip(input.xAxis, state.lookingRight);
        Move(input.xAxis, input.IsGrounded);
        UpdateJumpVariables(input);
        Jump(input);
        anim.SetFloat("Yelocity", rb.linearVelocity.y);
    }

    private void Move(float xAxis, bool IsGrounded)
    {
        //xelocity is the walkspeed times the direction of the x axis (-1 or 1), with yelocity staying as it was 
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
        //set to walk if velocity is greater/lesser than zero and player is on the ground
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && IsGrounded);
    }

    private void Flip(float xAxis, bool lookingRight)
    {
        if (xAxis == 0) return;

        transform.localScale = new Vector2(
            Mathf.Sign(xAxis) * Mathf.Abs(scaleX),
            transform.localScale.y
        );
        state.lookingRight = xAxis > 0;
    }

    private void Jump(PlayerInputHandler input)
    {
        //If the user is not in a state of jumping, and both counters are above zero
        if (!state.jumping && jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            state.jumping = true;
        }

        //Jump height will depend on how long user holds jump button, if released = velocity is back to zero
        if (input.JumpReleased && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            state.jumping = false;
        }
        anim.SetBool("Jumping", input.IsGrounded);
    }

    private void UpdateJumpVariables(PlayerInputHandler input)
    {
        if (input.IsGrounded)
        {
            state.jumping = false;
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (input.JumpPressed)
            jumpBufferCounter = jumpBufferFrames;
        else
            jumpBufferCounter--;
    }

}
