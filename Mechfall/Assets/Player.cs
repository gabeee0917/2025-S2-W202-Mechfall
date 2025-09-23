using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int hp = 3;
    private Boolean iframe = false;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage()
    {
        if (!iframe)
        {
            iframe = true;
            hp -= 1;
            Invoke(nameof(iFrameOver), 1f);
            
            Vector2 hitBack = new Vector2(2, 4);

            rb.linearVelocity = hitBack;
        }
    }

    private void iFrameOver() {
        iframe = false;
    }
}
