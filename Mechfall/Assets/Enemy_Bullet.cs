using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public float speed = 5;
    private float bulletDS = 10;

    // Bullet body
    private Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        // Move the bullet and despawn it after an allocated time of it in the air
        rb.linearVelocity = transform.right * speed;

        Invoke(nameof(DeSpawn), bulletDS);
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        Debug.Log(hitInfo.name);

        if (player != null)
        {
            player.takeDamage();
            Destroy(gameObject);
        }
    }

    void DeSpawn() {
        // Removes the game object
        Destroy(gameObject);
    }
}
