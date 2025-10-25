using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp = 5;
    public int phase = 1;
    public Transform testpoint;
    public GameObject testBox;

    void Start()
    {
        Instantiate(testBox, testpoint.position, testpoint.rotation);
    }

    public void TakeDamage(int d)
    {
        // Makes the object take damage and removes it if its hp is 0
        hp -= d;
        Debug.Log("Boss Test");

        if (hp <= 0)
        {
            PhaseCheck();
        }
    }

    void PhaseCheck()
    {
        if (phase == 3)
        {
            Die();
        }
        else phase -= 1;
    }

    void Die()
    {
        // Destroys the object
        Destroy(gameObject);
    }
}
