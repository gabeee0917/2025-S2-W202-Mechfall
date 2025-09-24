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
    public Boolean facingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void Update()
    {

        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
            return;
        }

        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else rb.linearVelocity = new Vector2(-speed, 0);

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
        
        facingLeft = !facingLeft;
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }

    public void PuaseMovement(float duration)
    {
        knockbackTime = duration;
    }
}
