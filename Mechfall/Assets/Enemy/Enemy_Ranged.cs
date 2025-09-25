using System;
using UnityEngine;

public class Enemy_Ranged : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D rb;
    public float speed = 2;

    public GameObject player;
    public Boolean facingRight = false;
    public float agroDistance = 10;
    public float range = 5;
    public float gunCDT = 1;
    public Transform firepoint;
    private Boolean isFiring = false;
    private Animator animator;
    private float knockbackTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        // Check where the player is relitive to the enemy and flip the enemy if needed
        float direction = player.transform.position.x - transform.position.x;
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            flip();
        }

        // Check if the player is within agro range and walk up to them just within shooting range
        if (Vector2.Distance(player.transform.position, transform.position) < agroDistance &&
        Vector2.Distance(player.transform.position, transform.position) > range - .5)
        {
            //Debug.Log("In Range");

            if (!facingRight)
            {
                rb.linearVelocity = new Vector2(-speed, 0);
            }
            else rb.linearVelocity = new Vector2(speed, 0);

        }

        // Check if the player is within shooting range 
        if (Vector2.Distance(player.transform.position, transform.position) < range)
        {
            // If the enemy is not already firing start firing and change animation
            if (!isFiring)
            {
                InvokeRepeating(nameof(Fire), 0f, gunCDT);
                animator.SetBool("Shooting", true);
            }
            isFiring = true;
        }

        else
        {
            // Stop the invoke for firing and change animation
            CancelInvoke(nameof(Fire));
            animator.SetBool("Shooting", false);
            isFiring = false;
        }
    }

    private void flip()
    {
        // Flip the enemy gameobject
        facingRight = !facingRight;
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }

    public void Fire()
    {
        // Create a bullet and fire it
        GameObject b = Instantiate(bullet, firepoint.position, firepoint.rotation);

        // Flip bullet direction if enemy is facing left
        if (!facingRight)
        {
            b.transform.right = -transform.right;
        }
    }
    
    public void PuaseMovement(float duration)
    {
        // Pause the movement for knockback
        knockbackTime = duration;
    }
}
