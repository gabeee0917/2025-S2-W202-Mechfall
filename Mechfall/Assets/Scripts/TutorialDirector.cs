using UnityEngine;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    public CanvasGroup blackPanel;   // Fade panel
    public TextMeshProUGUI tutorialText;
    public float fadeDuration = 2f;

    private bool pressedA = false;
    private bool pressedD = false;
    private bool pressedSpace = false;
    private int stage = 0;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        blackPanel.alpha = 0;

        // Stage 0: Welcome message
        tutorialText.text = "Welcome to the tutorial";
        yield return new WaitForSeconds(2f);

        // Move to movement tutorial
        stage = 1;
        tutorialText.text = "Press A to move Left and D to move Right";
    }

    void Update()
    {
        if (stage == 1)
        {
            if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
            if (Input.GetKeyDown(KeyCode.D)) pressedD = true;

            if (pressedA && pressedD)
            {
                stage = 2;
                tutorialText.text = "Press Space to Jump";
            }
        }
        else if (stage == 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressedSpace = true;
                tutorialText.text = ""; // Clear text (or trigger next stage)
                stage = 3; // tutorial complete for now
            }
        }
    }
}
