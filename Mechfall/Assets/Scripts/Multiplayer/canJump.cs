using UnityEngine;
using System.Collections;
public class canJump : MonoBehaviour
{
    // pseudo jump check for PVP using trigger colliders on the ground and walls instead of raycasting or overlaps

    // can jump when inside trigger zone, wing sprite disabled
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus ps = other.gameObject.GetComponent<PlayerStatus>();
            ps.jumpable = true;
            ps.wing.gameObject.SetActive(false);
        }
    }

    // cant jump when not in trigger zone, wing sprite enabled when not in zone (pseudo when in air)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus ps = other.gameObject.GetComponent<PlayerStatus>();
            ps.jumpable = false;
            ps.wing.gameObject.SetActive(true);
        }
    }
}
