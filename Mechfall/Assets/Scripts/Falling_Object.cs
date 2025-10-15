using System;
using UnityEngine;

public class Falling_Object : MonoBehaviour
{
    public GameObject warning;
    public Transform warning_point;
    private GameObject player;
    private Rigidbody2D rb;
    private Boolean triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 10 && !triggered)
        {
            triggered = true;
            Instantiate(warning, warning_point.position, warning_point.rotation);
            Invoke(nameof(fall), 1);
        }
    }

    void fall()
    {
        rb.gravityScale = 3;

    }
}
