using System;
using UnityEngine;

public class ClosingBox : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public Boolean right = true;
    public float despawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroySelf), despawn);
    }

    void Update()
    {
        if (right)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        } else rb.linearVelocity = new Vector2(-speed, 0);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        string player = hitInfo.tag;
        Debug.Log(hitInfo.name);

        if (player == "Player")
        {
            Debug.Log("hit");
            DestroySelf();
        }
    }
}
