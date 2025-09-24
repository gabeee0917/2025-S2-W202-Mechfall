using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 5;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        checkMoving(d);

        Vector2 hitBack = new Vector2((1 * d), (d * 2));

        rb.linearVelocity = hitBack;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        Debug.Log(hitInfo.name);

        if (player != null)
        {
            player.takeDamage();
        }
    }

    private void checkMoving(int d)
    {
        Enemy_Roaming roaming = GetComponent<Enemy_Roaming>();

        if (roaming != null)
        {
            roaming.PuaseMovement(.4f * d);
        }

        Enemy_Chaseing chaseing = GetComponent<Enemy_Chaseing>();

        if (chaseing != null)
        {
            chaseing.PuaseMovement(.4f * d);
        }
    }
}
