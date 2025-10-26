using UnityEngine;

public class Warning_Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Invoke(nameof(DeSpawn), 1);
    }

    // Update is called once per frame
    void DeSpawn()
    {
        Destroy(gameObject);
    }
}