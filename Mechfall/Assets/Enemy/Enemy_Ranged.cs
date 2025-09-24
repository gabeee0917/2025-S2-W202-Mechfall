using System;
using UnityEngine;

public class Enemy_Ranged : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D rb;
    public float speed = 2;

    public GameObject player;
    public Boolean facingLeft = false;
    public float agroDistance = 10;
    public float range = 5;
    public float gunCDT = 1;
    public Transform firepoint;
    private Boolean isFiring = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDistance = player.transform.position;
        Debug.Log(Vector2.Distance(player.transform.position, transform.position));


        float direction = player.transform.position.x - transform.position.x;

        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            flip();
        }

        if (Vector2.Distance(player.transform.position, transform.position) < agroDistance &&
        Vector2.Distance(player.transform.position, transform.position) > range - .5)
        {
            //Debug.Log("In Range");

            if (facingLeft)
            {
                rb.linearVelocity = new Vector2(-speed, 0);
            }
            else rb.linearVelocity = new Vector2(speed, 0);

        }

        if (Vector2.Distance(player.transform.position, transform.position) < range)
        {
            if (!isFiring)
            {
                InvokeRepeating(nameof(Fire), 0f, gunCDT);
            }
            isFiring = true;
        }

        else
        {
            CancelInvoke(nameof(Fire));
            isFiring = false;
        }
    }

    private void flip()
    {
        facingLeft = !facingLeft;
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }

    public void Fire()
    {
         GameObject b = Instantiate(bullet, firepoint.position, firepoint.rotation);

        // Flip bullet direction if enemy is facing left
        if (facingLeft)
        {
            b.transform.right = -transform.right;
        }
    }
}
