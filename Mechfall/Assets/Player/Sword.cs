using UnityEngine;

public class Sword : MonoBehaviour
{

    public int damage = 1;
    public Rigidbody2D rb;
    public float swordTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DeSpawn), swordTime);
    }



    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check for hitting something and remove itself when it dose
        Debug.Log(hitInfo.name);
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        // Call the object to take damage if it needs to
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void DeSpawn() {
        // Removes the game object
        Destroy(gameObject);
    }
}
