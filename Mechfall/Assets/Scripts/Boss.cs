using System;
using System.Collections;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject warning;
    private int last;
    Boolean hitable;

    void Start()
    {
        // Testing Functions
        //Instantiate(fallingObject, leftP.position, leftP.rotation);
        //Instantiate(fallingObject, rightp.position, rightp.rotation);
        //Instantiate(sideObject, rightC.position, rightC.rotation).GetComponent<ClosingBox>().right = false;
        //Instantiate(sideObject, leftC.position, leftC.rotation);
        //rightFall();
        //leftFall();
        //leftClose();
        //rightClose();
        //StartCoroutine(spawnEnemyLeft());
        //StartCoroutine(spawnEnemyRight());
        //phase = 2;

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
        phase += 1;
        hitable = false;
        if (phase == 3)
        {
            Die();
        }
        hp = 5;

        phaseLogic();
    }

    void Die()
    {
        // Destroys the object
        Destroy(gameObject);
    }

    void phaseLogic()
    {
        if (phase == 1)
        {
            Invoke(nameof(phaseOne), 0);
            Invoke(nameof(phaseOne), 7);
            Invoke(nameof(phaseOne), 14);
            Invoke(nameof(hitTrue), 21);
        }

        if (phase == 2)
        {
            Invoke(nameof(phaseTwo), 0);
            Invoke(nameof(phaseTwo), 5);
            Invoke(nameof(phaseTwo), 10);
            Invoke(nameof(hitTrue), 21);
        }
    }

    void hitTrue()
    {
        hitable = true;
    }

    void leftFall()
    {
        Vector2 position = new Vector2();
        for (float i = 2.5f; i <= 8.5f; i++)
        {
            position = new Vector2(-i, -3.6f);
            Instantiate(warning, position, leftC.rotation);
        }
        Invoke(nameof(summonLeftFall), 1f);
    }
    void summonLeftFall()
    {
        Instantiate(fallingObject, leftP.position, leftP.rotation);
    }

    void rightFall()
    {
        Vector2 position = new Vector2();
        for (float i = 2.5f; i <= 8.5f; i++)
        {
            position = new Vector2(i, -3.6f);
            Instantiate(warning, position, rightC.rotation);
        }
        Invoke(nameof(summonRightFall), 1f);
    }
    void summonRightFall()
    {
        Instantiate(fallingObject, rightp.position, rightp.rotation);
    }

    void leftClose()
    {
        Vector2 position = new Vector2();
        for (float i = 0.5f; i <= 8.5f; i++)
        {
            for (float j = 4.6f; j >= -3.6f; j--)
            {
                position = new Vector2(-i, j);
                Instantiate(warning, position, rightC.rotation);
            }
        }
        Invoke(nameof(summonLeftClose), 1f);
    }
    void summonLeftClose()
    {
        Instantiate(sideObject, leftC.position, leftC.rotation);
    }

    void rightClose()
    {
        Vector2 position = new Vector2();
        for (float i = 0.5f; i <= 8.5f; i++)
        {
            for (float j = 4.6f; j >= -3.6f; j--)
            {
                position = new Vector2(i, j);
                Instantiate(warning, position, rightC.rotation);
            }
        }
        Invoke(nameof(summonRightClose), 1f);
    }
    void summonRightClose()
    {
        Instantiate(sideObject, rightC.position, rightC.rotation).GetComponent<ClosingBox>().right = false;
    }

    private IEnumerator spawnEnemyLeft()
    {
        GameObject enemySpawned = Instantiate(enemy, eLeft.position, eLeft.rotation);
        enemySpawned.GetComponent<Enemy_Chaseing>().range = 100;
        enemySpawned.GetComponent<Enemy_Chaseing>().speed = 2;
        enemySpawned.GetComponent<Rigidbody2D>().gravityScale = 50;

        yield return new WaitForSeconds(1);
        enemySpawned.GetComponent<Rigidbody2D>().gravityScale = 1;
        Debug.Log("ran");
    }

    private IEnumerator spawnEnemyRight()
    {
        GameObject enemySpawned = Instantiate(enemy, eRight.position, eRight.rotation);
        enemySpawned.GetComponent<Enemy_Chaseing>().range = 100;
        enemySpawned.GetComponent<Enemy_Chaseing>().speed = 2;
        enemySpawned.GetComponent<Rigidbody2D>().gravityScale = 50;

        yield return new WaitForSeconds(1);
        enemySpawned.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    void phaseOne()
    {
        int current = Random.Range(1, 5);
        for (int i = 0; i <= 4; i += 4)
        {
            Debug.Log(current);
            switch (current)
            {
                case 1:
                    Invoke(nameof(rightFall), i);
                    break;
                case 2:
                    Invoke(nameof(rightClose), i);
                    break;
                case 3:
                    Invoke(nameof(leftFall), i);
                    break;
                case 4:
                    Invoke(nameof(leftClose), i);
                    break;
                default:
                    Debug.Log("out of range");
                    break;
            }
            last = current;
            switch (last)
            {
                case 1:
                    current = 4;
                    break;
                case 2:
                    current = Random.Range(3, 5);
                    break;
                case 3:
                    current = 2;
                    break;
                case 4:
                    current = Random.Range(1, 3);
                    break;
                default:
                    Debug.Log("out of range");
                    break;
            }
        }
    }

    void phaseTwo()
    {
        int current = Random.Range(1, 3);
        {
            for (int i = 0; i <= 3; i += 1)
            {
                switch (current)
                {
                    case 1:
                        Invoke(nameof(spawnLeft), i);
                        break;
                    case 2:
                        Invoke(nameof(spawnRight), i);
                        break;
                    default:
                        Debug.Log("out of range");
                        break;
                }
                current = Random.Range(1, 3);
            }
        }
    }

    void spawnLeft()
    {
        StartCoroutine(spawnEnemyLeft());
    }
    
    void spawnRight()
    {
        StartCoroutine(spawnEnemyRight());
    }

}
