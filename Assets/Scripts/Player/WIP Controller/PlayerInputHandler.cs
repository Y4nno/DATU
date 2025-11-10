using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float xAxis { get; private set; }
    public float yAxis { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool DashPressed { get; private set; }

    public bool IsGrounded { get; private set; }

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        JumpPressed = Input.GetButtonDown("Jump");
        JumpReleased = Input.GetButtonUp("Jump");
        AttackPressed = Input.GetMouseButtonDown(0);
        DashPressed = Input.GetButtonDown("Dash");
        IsGrounded = Grounded();
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
