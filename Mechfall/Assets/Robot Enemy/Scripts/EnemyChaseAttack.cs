using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class EnemyChaseAttack : MonoBehaviour
{
    [Header("Target")]
    public Transform player;           // Drag your chibi-robot here

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float followRange = 20f;    // start moving if within this
    public float stopDistance = 0.6f;  // how close before we stop (for attack)

    [Header("Attack")]
    public float attackRange = 1.2f;   // distance to trigger an attack
    public float attackCooldown = 1.0f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    float cooldownTimer;

    // Animator parameter hashes (must match the Animator)
    static readonly int HashDrill = Animator.StringToHash("Drill");
    static readonly int HashKick  = Animator.StringToHash("Kick");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr  = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!player) return;

        cooldownTimer -= Time.deltaTime;

        // Horizontal vector to player
        float dx = player.position.x - transform.position.x;
        float dist = Mathf.Abs(dx);

        // Face the player (flip if your art faces right by default)
        bool faceRight = dx > 0f;
        sr.flipX = !faceRight; // flip if looking left; invert if wrong

        // Close enough to move?
        if (dist <= followRange && dist > stopDistance)
        {
            float dir = Mathf.Sign(dx);
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        // Attack when close & off cooldown
        if (dist <= attackRange && cooldownTimer <= 0f)
        {
            // stop to attack
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            // 50/50 pick
            if (Random.value < 0.5f) anim.SetTrigger(HashDrill);
            else                     anim.SetTrigger(HashKick);

            cooldownTimer = attackCooldown;
        }
    }

    // Visualize ranges in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
