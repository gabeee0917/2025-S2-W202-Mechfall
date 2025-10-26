using System;
using UnityEngine;

public class Falling_Object : MonoBehaviour
{
    public GameObject warning;
    public Transform warning_point;
    public double range = 7;
    private GameObject player;
    private Rigidbody2D rb;
    private Boolean triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        Debug.Log(hitInfo.name);

        if (player != null)
        {
            player.takeDamage();
            Destroy(gameObject);
        } else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector2.Distance(player.transform.position, transform.position));
        if (Vector2.Distance(player.transform.position, transform.position) < range && !triggered)
        {
            triggered = true;
            Instantiate(warning, warning_point.position, warning_point.rotation);
            Invoke(nameof(Fall), 1);
        }
    }

    void Fall()
    {
        rb.gravityScale = 3;

    }
}
