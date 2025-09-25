using System;
using UnityEngine;

public class Enemy_Chaseing : MonoBehaviour
{

    public float speed = 2f;
    public GameObject player;
    public Boolean facingRight = true;
    private float knockbackTime;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop the enemy from moving during knockback
        if (knockbackTime > 0)
        {
            knockbackTime -= Time.deltaTime;
            return;
        }

        // Testing
        //Vector3 playerDistance = player.transform.position;
        //Debug.Log(Vector2.Distance(player.transform.position, transform.position));

        if (Vector2.Distance(player.transform.position, transform.position) < 10)
        {
            //Debug.Log("In Range");

            // Get the enemys location relitive to the player and flip enemy if needed
            float direction = player.transform.position.x - transform.position.x;
            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
            {
                flip();
            }

            // Make the enmy move the direction its facing
            if (!facingRight)
            {
                rb.linearVelocity = new Vector2(-speed, 0);
            }
            else rb.linearVelocity = new Vector2(speed, 0);

        } //else Debug.Log("Not in Range");

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
        // Pause the movement for knockback
        knockbackTime = duration;
    }
}
