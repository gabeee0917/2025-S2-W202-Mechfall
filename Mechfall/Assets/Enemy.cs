using System.Reflection.Emit;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 5;
    public Rigidbody2D rb;

    public void TakeDamage(int d)
    {
        // Makes the object take damage and removes it if its hp is 0
        hp -= d;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        Vector2 hitBack = (Vector2.up * (d * 2)) - ((Vector2)transform.right * d);

        rb.AddForce(hitBack, ForceMode2D.Impulse);

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroys the object
        Destroy(gameObject);
    }

}
