using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject player;
    public string scenename;

    void Update()
    {
        if (playerInRange && InputManager.PlayerInput.actions["Interact"].triggered)
        {
            SceneManager.LoadScene(scenename); 
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
