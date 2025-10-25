using System;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp = 5;
    public int phase = 1;
    public Transform leftP;
    public Transform rightp;
    public Transform leftC;
    public Transform rightC;
    public Transform eRight;
    public Transform eLeft;
    public GameObject enemy;
    public GameObject fallingObject;
    public GameObject sideObject;
    Boolean hitable;

    void Start()
    {
        //Instantiate(fallingObject, leftP.position, leftP.rotation);
        //Instantiate(fallingObject, rightp.position, rightp.rotation);
        Instantiate(sideObject, rightC.position, rightC.rotation).GetComponent<ClosingBox>().right = false;
        Instantiate(sideObject, leftC.position, leftC.rotation);
        
        hitable = false;
        phaseLogic();
    }

    public void TakeDamage(int d)
    {
        if (hitable)
        {
            // Makes the object take damage and removes it if its hp is 0
            hp -= d;
            Debug.Log("Boss Test");

            if (hp <= 0)
            {
                PhaseCheck();
            }
        }
    }

    void PhaseCheck()
    {
        if (phase == 3)
        {
            Die();
        }

        phase += 1;
        
        phaseLogic();
    }

    void Die()
    {
        // Destroys the object
        Destroy(gameObject);
    }

    void phaseLogic()
    {
        
    }
}
