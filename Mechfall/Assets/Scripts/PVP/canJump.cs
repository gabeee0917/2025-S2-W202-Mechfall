using UnityEngine;
using System.Collections;
public class canJump : MonoBehaviour
{
    // pseudo jump check for PVP using trigger colliders on the ground and walls
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus ps = other.gameObject.GetComponent<PlayerStatus>();
            ps.jumpable = true;
            ps.wing.gameObject.SetActive(false);
        }
    }
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
