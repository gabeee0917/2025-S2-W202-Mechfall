using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class IntroTitle : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup blackBackground;   // full-screen black panel
    public CanvasGroup titleGroup;        // container (text + image)
    public TMP_Text titleText;

    [Header("Content")]
    public string title = "MECHFALL";
    public float titleFontSize = 96f;

    [Header("Timing (seconds)")]
    public float delayBeforeStart = 0.2f;
    public float fadeInDuration = 0.9f;
    public float holdDuration = 1.1f;
    public float fadeOutDuration = 0.9f;

    [Header("Behavior")]
    public bool allowSkip = true;         // any key / mouse click to skip
    public UnityEvent OnIntroFinished;

    bool _running;

    void Awake()
    {
        if (titleText)
        {
            titleText.text = title;
            titleText.fontSize = titleFontSize;
        }

        if (blackBackground) blackBackground.alpha = 1f;    // start fully black
        if (titleGroup) titleGroup.alpha = 0f;              // title hidden
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        if (!_running) StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        _running = true;

        if (delayBeforeStart > 0f)
            yield return new WaitForSecondsRealtime(delayBeforeStart);

        // === FADE IN ===
        yield return Fade(titleGroup, 0f, 1f, fadeInDuration);

        // === HOLD ===
        if (holdDuration > 0f)
            yield return new WaitForSecondsRealtime(holdDuration);

        // === FADE OUT ===
        // 1) Fade out the title while keeping black fully visible
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = 1f - Mathf.Clamp01(t / fadeOutDuration);
            if (titleGroup) titleGroup.alpha = a;
            if (blackBackground) blackBackground.alpha = 1f;  // keep black solid
            if (allowSkip && (Input.anyKeyDown || Input.GetMouseButtonDown(0))) break;
            yield return null;
        }

        if (titleGroup) titleGroup.alpha = 0f;
        if (blackBackground) blackBackground.alpha = 1f;

        // 2) Now fade black away to reveal scene
        yield return Fade(blackBackground, 1f, 0f, 0.35f);

        OnIntroFinished?.Invoke();
        gameObject.SetActive(false);
        _running = false;
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float dur)
    {
        if (!cg || dur <= 0f)
        {
            if (cg) cg.alpha = to;
            yield break;
        }

        float t = 0f;
        cg.alpha = from;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / dur);

            if (allowSkip && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
            {
                cg.alpha = to;
                break;
            }

            yield return null;
        }

        cg.alpha = to;
    }
}
