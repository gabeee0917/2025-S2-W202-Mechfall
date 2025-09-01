using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.velocity = transform.right * speed;
    }


}
