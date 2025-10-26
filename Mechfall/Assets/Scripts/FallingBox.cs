using UnityEditor.Rendering;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DestroySelf), 3f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        string player = hitInfo.tag;
        Debug.Log(hitInfo.name);

        if (player == "Player")
        {
            Debug.Log("hit");
            DestroySelf();
        }
    }
}
