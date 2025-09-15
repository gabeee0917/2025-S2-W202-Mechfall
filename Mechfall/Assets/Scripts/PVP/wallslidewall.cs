using UnityEngine;

using System.Collections;

// pseudo wallslide using trigger colliders. If player enters, make the player's wallslide animation parameter true and on exit false
public class WallslideanimationWall : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus ps = other.gameObject.GetComponent<PlayerStatus>();
            ps.WallslideOn();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            PlayerStatus ps = other.gameObject.GetComponent<PlayerStatus>();
            ps.WallslideOff();
        }
    }

}