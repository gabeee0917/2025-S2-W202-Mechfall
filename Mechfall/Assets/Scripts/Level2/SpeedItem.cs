using UnityEngine;
using System;

public class SpeedItem : MonoBehaviour, IItem
{
    public static event Action<float> onSpeedCollected;
    public float speedMult = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
    
    public void Collect()
    {
        //notify Player Movement
        onSpeedCollected.Invoke(speedMult);
        Destroy(gameObject);
    }
}
