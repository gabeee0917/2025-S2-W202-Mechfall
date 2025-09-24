using UnityEngine;

/// <summary>
/// Simple 2D teleporter:
/// - Place on a portal GameObject that has a SpriteRenderer + BoxCollider2D (isTrigger).
/// - Pair two portals by assigning each other's Transform to `partner`.
/// - Assign `playerTarget` (or leave null and it will find the object tagged "Player").
/// - After a successful teleport, it calls director.NotifyTeleported().
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Teleport : MonoBehaviour
{
    [Header("Links")]
    public Transform playerTarget;          // Player transform; found by tag "Player" if left null.
    public Transform partner;               // The other portal's transform.
    public TutorialDirector director;       // Set by the director when spawning portals.

    [Header("Behavior")]
    [Tooltip("Seconds before this portal can teleport the player again (prevents ping-pong).")]
    public float cooldown = 0.25f;

    [Tooltip("How far in front of the destination portal the player appears.")]
    public float exitOffset = 0.35f;

    [Tooltip("Minimum horizontal speed pushed out of the exit (keeps momentum).")]
    public float minExitSpeed = 3.0f;

    float lastTeleportTime = -999f;

    void Awake()
    {
        // Auto-find player if not wired in Inspector
        if (playerTarget == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
        }

        // Make sure our trigger is really a trigger
        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    void OnValidate()
    {
        // Keep the collider a trigger in edit time too
        var col = GetComponent<BoxCollider2D>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Cooldown: avoid immediate ping-pong
        if (Time.time - lastTeleportTime < cooldown) return;

        // Only teleport the player
        var isPlayer = (other.attachedRigidbody != null && other.attachedRigidbody.transform == playerTarget)
                       || other.CompareTag("Player");

        if (!isPlayer) return;
        if (partner == null) return;

        // Compute destination just in front of the partner, based on its facing.
        // Using partner.right lets flipped scale determine exit direction automatically.
        Vector3 exitDir = partner.right.normalized;
        Vector3 dest = partner.position + exitDir * exitOffset;

        // Move the player & give them a nudge out of the exit
        var rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.position = dest;

            // Keep vertical velocity; ensure outward push horizontally
            Vector2 v = rb.linearVelocity;
            v.x = Mathf.Sign(exitDir.x) * Mathf.Max(Mathf.Abs(v.x), minExitSpeed);
            rb.linearVelocity = v;
        }
        else
        {
            other.transform.position = dest;
        }

        // Start cooldown on both portals
        lastTeleportTime = Time.time;
        var partnerTp = partner.GetComponent<Teleport>();
        if (partnerTp != null) partnerTp.lastTeleportTime = Time.time;

        // Let the director know a teleport happened (used to dismiss tutorial text)
        if (director != null) director.NotifyTeleported();
    }

#if UNITY_EDITOR
    // Helpful gizmo to see the exit point/direction in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var to = (partner != null ? partner.position + partner.right.normalized * exitOffset
                                  : transform.position + transform.right * exitOffset);
        Gizmos.DrawLine(transform.position, to);
        Gizmos.DrawSphere(to, 0.04f);
    }
#endif
}
