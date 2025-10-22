using System.Text.RegularExpressions;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public bool isActivated = false;
    public SwitchManager manager;
    [SerializeField] private AudioClip capture;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            manager.SwitchActivated();
            SoundManager.instance.PlayCapture();
        }
    }
}
