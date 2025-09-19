using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Chanagble bullet varibles
    public float speed = 20;
    public int damage = 1;
    public float bulletDS = 10;

    // Bullet body
    public Rigidbody2D rb;

    void Start()
    {
        // Move the bullet and despawn it after an allocated time of it in the air
        rb.linearVelocity = transform.right * speed;

        Invoke(nameof(DeSpawn), bulletDS);
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check for hitting something and remove itself when it dose
        Debug.Log(hitInfo.name);
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        // Call the object to take damage if it needs to
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    void DeSpawn() {
        // Removes the game object
        Destroy(gameObject);
    }

}
