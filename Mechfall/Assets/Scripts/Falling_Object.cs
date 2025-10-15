using UnityEngine;

public class Falling_Object : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < .5)
        {
            rb.gravityScale = 3;
        }
    }
}
