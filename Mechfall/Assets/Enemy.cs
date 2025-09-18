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

        HitBackwards(d);

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

    void HitBackwards(int d)
    {

        Vector2 hitBack = new Vector2((1 * d), (d*2));

        rb.AddForce(hitBack, ForceMode2D.Impulse);
    }

}
