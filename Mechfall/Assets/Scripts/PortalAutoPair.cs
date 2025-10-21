using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Teleport))]
public class PortalAutoPair : MonoBehaviour
{
    [Header("Linking")]
    [Tooltip("Two portals with the same key will auto-link.")]
    public string pairKey = "A";

    [Tooltip("If set, overrides key-based pairing and links to this Transform directly.")]
    public Transform partnerOverride;

    [Tooltip("If true, set the other portal's partner back to this one (two-way).")]
    public bool twoWay = true;

    [Header("Player assignment")]
    public bool autoAssignPlayer = true;
    public string playerTag = "Player";

    // runtime
    static readonly Dictionary<string, List<PortalAutoPair>> groups = new();
    Teleport tp;

    void Awake()
    {
        tp = GetComponent<Teleport>();
    }

    void OnEnable()
    {
        if (!groups.TryGetValue(pairKey, out var list))
            groups[pairKey] = list = new List<PortalAutoPair>();
        if (!list.Contains(this)) list.Add(this);

        TryLink();
    }

    void OnDisable()
    {
        if (groups.TryGetValue(pairKey, out var list))
        {
            list.Remove(this);
            if (list.Count == 0) groups.Remove(pairKey);
        }
    }

    void Start()
    {
        if (autoAssignPlayer && tp.playerTarget == null)
        {
            var player = GameObject.FindGameObjectWithTag(playerTag);
            if (player) tp.playerTarget = player.transform;
        }
    }

    void TryLink()
    {
        if (tp == null) tp = GetComponent<Teleport>();

        // Direct override wins
        if (partnerOverride)
        {
            tp.partner = partnerOverride;
            if (twoWay)
            {
                var otherTP = partnerOverride.GetComponent<Teleport>();
                if (otherTP) otherTP.partner = this.transform;
            }
            return;
        }

        if (!groups.TryGetValue(pairKey, out var list)) return;

        // find another portal in this group that isn’t us and link if possible
        foreach (var other in list)
        {
            if (other == this) continue;
            var otherTP = other.GetComponent<Teleport>();
            if (otherTP == null) continue;

            bool canLink = tp.partner == null || otherTP.partner == null;
            if (!canLink) continue;

            tp.partner = other.transform;
            if (twoWay) otherTP.partner = this.transform;

            if (list.Count > 2)
                Debug.LogWarning($"[PortalAutoPair] More than two portals share key '{pairKey}'. " +
                                 "First unlinked pair were connected; consider unique keys per pair.", this);
            break;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        var t = GetComponent<Teleport>();
        if (t && t.partner)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.8f);
            Gizmos.DrawLine(transform.position, t.partner.position);
            Gizmos.DrawSphere(transform.position, 0.06f);
            Gizmos.DrawSphere(t.partner.position, 0.06f);
        }
    }
#endif
}
