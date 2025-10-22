using System.Text.RegularExpressions;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public bool isActivated = false;
    public SwitchManager manager;
    private SpriteRenderer spriteRenderer;
    public Color activeColor = Color.green;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            spriteRenderer.color = activeColor;
            isActivated = true;
            manager.SwitchActivated();
        }
    }
}
