using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20;
    public int damage = 1;
    public Rigidbody2D rb;
    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
