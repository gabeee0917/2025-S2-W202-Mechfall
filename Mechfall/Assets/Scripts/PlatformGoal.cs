using UnityEngine;

public class PlatformGoal : MonoBehaviour
{
    [HideInInspector] public TutorialDirector director;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Trigger the neon pulse
        var pulse = GetComponentInParent<PlatformOutlinePulse>();
        if (pulse) pulse.PlayFlash();

        // 2) Notify the tutorial director
        if (director) director.OnGoalReached();
    }
}

