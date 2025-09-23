using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject player;

    public string scenename;



    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (playerInRange && InputManager.PlayerInput.actions["Interact"].triggered)
        {
            player = GameObject.FindWithTag("Player");
            SceneManager.LoadScene(scenename); 
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player != null)
        {
            GameObject spawn = GameObject.Find("PlayerSpawn");
            if (spawn != null)
            {
                player.transform.position = spawn.transform.position;
            }
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
