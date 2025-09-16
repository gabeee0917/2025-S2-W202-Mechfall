using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject player;

    public string scenename;



    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.UpArrow))
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
