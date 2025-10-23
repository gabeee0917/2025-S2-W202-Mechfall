using UnityEngine;
using System.Collections;

/// <summary>
/// 2D Teleport with per-player immunity and per-portal tint kept via MaterialPropertyBlock.
/// - Keeps each portal's own SpriteRenderer tint (set in the Inspector).
/// - Dims entry and exit portals during cooldown, then fades back to their base tint.
/// - Teleports the player safely outside the partner portal in its up direction.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    [Header("Links")]
    public Transform playerTarget;        // player (auto-found by tag "Player" if null)
    public Transform partner;             // destination portal (Transform with Teleport)
    public TutorialDirector director;     // optional callback

    [Header("Behavior")]
    [Tooltip("Seconds the player ignores all portals after a teleport.")]
    public float cooldown = 3f;

    [Tooltip("Extra distance beyond the exit trigger to place the player.")]
    public float exitOffset = 0.25f;

    [Tooltip("If exiting velocity along partner.up is lower than this, we add more push.")]
    public float minExitSpeed = 3f;

    [Header("Visual (keep your own tint)")]
    [Tooltip("Sprite that should dim/fade. Leave empty to auto-find the first child SpriteRenderer.")]
    public SpriteRenderer glowSprite;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("How dark the portal looks during cooldown (multiplies the base tint).")]
    private float cooldownDim = 0.55f;

    // --- internal ---
    private Collider2D myTrigger;

    // base tint read from SpriteRenderer so each portal can use its own color set in Inspector
    private Color _baseTint = Color.white;

    // MPB so we don't touch shared materials and don't allocate per-frame
    private MaterialPropertyBlock _mpb;

    void Awake()
    {
        myTrigger = GetComponent<Collider2D>();
        myTrigger.isTrigger = true;

        // auto-find player
        if (playerTarget == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;
        }

        // auto-find sprite if not set
        if (glowSprite == null)
            glowSprite = GetComponentInChildren<SpriteRenderer>();

        // capture the base tint from whatever you set in the SpriteRenderer
        if (glowSprite != null)
        {
            _baseTint = glowSprite.color;
            _mpb = new MaterialPropertyBlock();
            ApplyTint(_baseTint); // ensure renderer starts at its own color via MPB
        }
    }

    void OnValidate()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (partner == null) return;

        // Only the player can trigger
        bool isPlayer =
            (other.attachedRigidbody != null && playerTarget != null && other.attachedRigidbody.transform == playerTarget)
            || other.CompareTag("Player");

        if (!isPlayer) return;

        // per-player immunity (prevents instant re-entry)
        var immunity = GetOrAddImmunity(other);
        if (immunity.Active) return;

        // compute safe exit beyond partner.up
        Vector2 exitDir = partner.up.normalized;
        float safeExtra = exitOffset;

        var partnerCol = partner.GetComponent<Collider2D>();
        if (partnerCol != null)
        {
            var ext = partnerCol.bounds.extents;
            safeExtra += Mathf.Max(ext.x, ext.y) + 0.05f;
        }
        else
        {
            safeExtra += 0.5f;
        }

        Vector2 safePos = (Vector2)partner.position + exitDir * safeExtra;

        // move and push out
        var rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.position = safePos;
            Vector2 v = rb.linearVelocity;
            float along = Vector2.Dot(v, exitDir);
            if (along < minExitSpeed) v += exitDir * (minExitSpeed - along);
            rb.linearVelocity = v;
        }
        else
        {
            other.transform.position = safePos;
        }

        // apply immunity window
        immunity.Set(cooldown);

        // dim entry + exit portals
        TriggerGlowCooldown();
        var partnerTp = partner.GetComponent<Teleport>();
        if (partnerTp != null) partnerTp.TriggerGlowCooldown();

        // optional tutorial hook
        if (director != null) director.NotifyTeleported();
    }

    // ---------- visual ----------
    /// <summary>Public so the partner can trigger our dim/fade externally.</summary>
    public void TriggerGlowCooldown()
    {
        if (!gameObject.activeInHierarchy) return;
        if (glowSprite == null) return;
        StartCoroutine(GlowCooldownRoutine());
    }

    IEnumerator GlowCooldownRoutine()
    {
        Color dim = _baseTint * cooldownDim;

        ApplyTint(dim);
        yield return new WaitForSeconds(cooldown);

        // fade back to base tint
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // fade speed
            ApplyTint(Color.Lerp(dim, _baseTint, t));
            yield return null;
        }
        ApplyTint(_baseTint);
    }

    void ApplyTint(Color c)
    {
        if (glowSprite == null) return;
        if (_mpb == null) _mpb = new MaterialPropertyBlock();

        glowSprite.GetPropertyBlock(_mpb);
        _mpb.SetColor("_Color", c);          // works with Sprite shaders
        glowSprite.SetPropertyBlock(_mpb);
    }

    // ---------- immunity helper ----------
    TeleportImmunity GetOrAddImmunity(Collider2D other)
    {
        var host = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;
        var tpi = host.GetComponent<TeleportImmunity>();
        if (tpi == null) tpi = host.gameObject.AddComponent<TeleportImmunity>();
        return tpi;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 dir = partner ? partner.up : transform.up;
        Vector3 to = (partner != null ? partner.position + dir * exitOffset
                                      : transform.position + dir * exitOffset);
        Gizmos.DrawLine(transform.position, to);
        Gizmos.DrawSphere(to, 0.05f);
    }
#endif
}

/// <summary>
/// Component stored on the player to ignore portals for a time window after teleport.
/// </summary>
public class TeleportImmunity : MonoBehaviour
{
    float untilTime = -1f;
    public bool Active => Time.time < untilTime;
    public void Set(float seconds) => untilTime = Time.time + Mathf.Max(0f, seconds);
}
