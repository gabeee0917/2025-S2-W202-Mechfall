using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lastStepTime = 0f;
    public float stepCooldown = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PlayFootstepSound()
    {
        if (!IsMoving()) return;

        if (Time.time - lastStepTime < stepCooldown) return;

        lastStepTime = Time.time;
        SoundManager.instance.PlayFootsteps();
    }

    private bool IsMoving()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 0.1f;
    }
}
