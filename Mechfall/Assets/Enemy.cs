using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 5;


    public void TakeDamage(int d)
    {
        // Makes the object take damage and removes it if its hp is 0
        hp -= d;

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
