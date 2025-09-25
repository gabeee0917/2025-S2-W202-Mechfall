using System;
using UnityEngine;

public class Enemy_Roaming : MonoBehaviour
{

    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Transform currentPoint;
    private float knockbackTime;
    public Boolean facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        
        // Stop the enemy from moving during knockback
        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
            return;
        }

        //Vector2 point = currentPoint.position - transform.position;
        // Cheeck if the enemy has reached the point and if it has move away from it
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else rb.linearVelocity = new Vector2(-speed, 0);

        // Check if the enemy has reached the point and if it has flip it
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
    }

    private void flip()
    {
        // Flip the enemy
        facingRight = !facingRight;
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }

    public void PuaseMovement(float duration)
    {
        // Pause anemy movement while being knocked back
        knockbackTime = duration;
    }
}
