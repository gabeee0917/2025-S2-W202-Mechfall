using System;
using UnityEngine;

public class Enemy_Chaseing : MonoBehaviour
{

    public float speed = 2f;
    public GameObject player;
    public Boolean facingPlayer = false;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDistance = player.transform.position;
        Debug.Log(Vector2.Distance(player.transform.position, transform.position));

        if (Vector2.Distance(player.transform.position, transform.position) < 10)
        {
            //Debug.Log("In Range");

            rb.linearVelocity = new Vector2(-speed, 0);
            
        } //else Debug.Log("Not in Range");

    }

    private void flip()
    {
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }
}
