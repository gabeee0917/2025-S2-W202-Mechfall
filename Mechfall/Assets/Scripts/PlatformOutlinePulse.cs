using UnityEngine;
using System.Collections;

public class PlatformOutlinePulse : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer outline;      // drag your Outline's SpriteRenderer here in the prefab

    [Header("Pulse Settings")]
    public int flashes = 2;
    public float flashDuration = 0.25f; // time for one up/down
    public float maxAlpha = 1.0f;       // peak brightness
    public float holdOnPeak = 0.05f;    // optional hold at brightest

    bool pulsing;

    void Reset()
    {
        // Auto-find a child named "Outline" if possible
        if (!outline)
        {
            var child = transform.Find("Outline");
            if (child) outline = child.GetComponent<SpriteRenderer>();
        }
    }

    public void PlayFlash()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!outline) return;

        StopAllCoroutines();
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        pulsing = true;
        outline.enabled = true;

        Color c = outline.color;

        for (int i = 0; i < flashes; i++)
        {
            // fade up
            float t = 0f;
            while (t < flashDuration)
            {
                t += Time.deltaTime;
                float a = Mathf.Clamp01(t / flashDuration);
                c.a = Mathf.Lerp(0f, maxAlpha, a);
                outline.color = c;
                yield return null;
            }

            if (holdOnPeak > 0f) yield return new WaitForSeconds(holdOnPeak);

            // fade down
            t = 0f;
            while (t < flashDuration)
            {
                t += Time.deltaTime;
                float a = Mathf.Clamp01(t / flashDuration);
                c.a = Mathf.Lerp(maxAlpha, 0f, a);
                outline.color = c;
                yield return null;
            }
        }

        // turn off after pulse
        c.a = 0f;
        outline.color = c;
        outline.enabled = false;
        pulsing = false;
    }
}

