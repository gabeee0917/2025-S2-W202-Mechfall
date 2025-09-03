using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 5;


    public void TakeDamage(int d)
    {
        hp -= d;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
