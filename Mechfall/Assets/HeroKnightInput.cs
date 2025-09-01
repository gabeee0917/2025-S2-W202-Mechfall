using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class HeroKnightInput : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    [Header("Ground Check")]
    public Transform groundCheck;         // empty child at the feet
    public LayerMask groundLayer;         // set to your Ground layer
    public float groundRadius = 0.15f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // Animator parameter names (match your Parameters tab)
    static readonly int HashGrounded  = Animator.StringToHash("Grounded");
    static readonly int HashAirSpeedY = Animator.StringToHash("AirSpeedY");
    static readonly int HashAnimState = Animator.StringToHash("AnimState");
    static readonly int HashJump      = Animator.StringToHash("Jump");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr  = GetComponent<SpriteRenderer>();

        rb.gravityScale = 5f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // horizontal input
        float x = Input.GetAxisRaw("Horizontal");

        // move
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // face move direction
        if (x != 0) sr.flipX = x < 0;

        // jump
        bool grounded = IsGrounded();
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger(HashJump);   // drives Jump state
        }

        // ---- Animator parameters ----
        anim.SetBool(HashGrounded, grounded);
        anim.SetFloat(HashAirSpeedY, rb.linearVelocity.y);

        // Simple AnimState: 0 = idle, 1 = run
        int animState = Mathf.Abs(x) > 0.1f ? 1 : 0;
        anim.SetInteger(HashAnimState, animState);
    }

    bool IsGrounded()
    {
        if (!groundCheck) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}

