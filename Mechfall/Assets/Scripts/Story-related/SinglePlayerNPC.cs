using UnityEngine;

public class SinglePlayerNPC : MonoBehaviour
{
    public GameObject textboximage;
    private bool overlap = false;

    void Update()
    {
        if (overlap && Input.GetKeyDown(KeyCode.F))
        {
            textboximage.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            overlap = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            overlap = false;
            textboximage.SetActive(false);
        }
    }
}
