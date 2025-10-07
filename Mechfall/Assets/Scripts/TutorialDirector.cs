using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup blackPanel;            // CanvasGroup on BlackPanel (alpha drives fade)
    public TextMeshProUGUI tutorialText;      // TMP text for prompts
    public float fadeDuration = 1.5f;         // initial fade-in duration

    [Header("Platform Step")]
    public GameObject platformPrefab;         // TutorialPlatform prefab (has PlatformGoal + optional outline pulse)
    public Transform platformSpawnPoint;      // Where the platform appears

    [Header("Portals")]
    public GameObject portalPrefab;           // Animated portal prefab (has Teleport + BoxCollider2D)
    public Transform groundPortalSpawnPoint;  // Ground-side spawn
    public Transform platformPortalSpawnPoint;// Platform-side spawn

    [Header("Player (optional)")]
    public Transform player;                  // If null, found by tag "Player" at runtime

    // runtime state
    bool pressedLeft, pressedRight;
    bool goalReached;
    bool teleportedOnce;

    void Awake()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        // 1) Fade from black
        yield return StartCoroutine(FadeFromBlack());

        // 2) Welcome
        yield return ShowMessage("Welcome to the Tutorial!", 1.25f);

        // 3) Move left & right
        yield return ShowMessage("Press ← to move Left and → to move Right", 0f);
        yield return WaitForMoveKeys();
        ClearMessage();

        // 4) Jump
        yield return ShowMessage("Press Space to jump", 0f);
        yield return WaitForJump();
        ClearMessage();

        // 5) Spawn practice platform & wait for landing
        SpawnPlatform();
        yield return ShowMessage("Try Jumping on the platform", 0f);
        yield return new WaitUntil(() => goalReached);
        ClearMessage();

        // 6) Spawn & link portals (bi-directional); flip ground one inward
        if (!SpawnAndLinkPortals())
        {
            Debug.LogWarning("TutorialDirector: Portal prefab or spawn points not set.");
            yield break;
        }

        // 7) Portal instruction; wait for the first successful teleport
        teleportedOnce = false;
        yield return ShowMessage("Entering one of the portals will lead you out the other", 0f);
        yield return new WaitUntil(() => teleportedOnce);
        ClearMessage();

        // 8) Congratulate, then fade to black over 5 seconds (text remains while fading)
        yield return ShowMessage("Well done!\nPress the button to leave", 0f);
        yield return StartCoroutine(FadeToBlack(5f));
        ClearMessage();

        // (Optional) load next scene or continue flow here…
        // SceneManager.LoadScene("NextScene");
    }

    // -----------------------
    // Fades
    // -----------------------
    IEnumerator FadeFromBlack()
    {
        if (blackPanel == null)
            yield break;

        blackPanel.alpha = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        blackPanel.alpha = 0f;
    }

    IEnumerator FadeToBlack(float seconds)
    {
        if (blackPanel == null)
            yield break;

        float start = blackPanel.alpha;
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(start, 1f, t / seconds);
            yield return null;
        }
        blackPanel.alpha = 1f;
    }

    // -----------------------
    // UI helpers
    // -----------------------
    IEnumerator ShowMessage(string msg, float holdSeconds)
    {
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = msg;
        }

        if (holdSeconds > 0f)
            yield return new WaitForSeconds(holdSeconds);
    }

    void ClearMessage()
    {
        if (tutorialText != null)
        {
            tutorialText.text = "";
            tutorialText.gameObject.SetActive(false);
        }
    }

    // -----------------------
    // Input gates
    // -----------------------
    IEnumerator WaitForMoveKeys()
    {
        pressedLeft = pressedRight = false;
        while (!(pressedLeft && pressedRight))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) pressedLeft = true;
            if (Input.GetKeyDown(KeyCode.RightArrow)) pressedRight = true;
            yield return null;
        }
    }

    IEnumerator WaitForJump()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
    }

    // -----------------------
    // Platform
    // -----------------------
    void SpawnPlatform()
    {
        if (platformPrefab == null || platformSpawnPoint == null)
        {
            Debug.LogWarning("TutorialDirector: Platform prefab or spawn point not set.");
            return;
        }

        var plat = Instantiate(platformPrefab, platformSpawnPoint.position, Quaternion.identity);

        // Notify when player lands
        var goal = plat.GetComponentInChildren<PlatformGoal>();
        if (goal != null) goal.director = this;
    }

    // Called by PlatformGoal when player touches the platform
    public void OnGoalReached()
    {
        goalReached = true;
    }

    // -----------------------
    // Portals
    // -----------------------
    bool SpawnAndLinkPortals()
    {
        if (portalPrefab == null || groundPortalSpawnPoint == null || platformPortalSpawnPoint == null)
            return false;

        var groundPortalGO   = Instantiate(portalPrefab,   groundPortalSpawnPoint.position,   Quaternion.identity);
        var platformPortalGO = Instantiate(portalPrefab, platformPortalSpawnPoint.position, Quaternion.identity);

        // Flip ONLY the ground portal horizontally so it faces inward
        var s = groundPortalGO.transform.localScale;
        groundPortalGO.transform.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);

        var groundTP   = groundPortalGO.GetComponent<Teleport>();
        var platformTP = platformPortalGO.GetComponent<Teleport>();
        if (groundTP == null || platformTP == null)
        {
            Debug.LogWarning("TutorialDirector: Portal prefab is missing Teleport component.");
            return false;
        }

        // Player target
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        groundTP.playerTarget   = player;
        platformTP.playerTarget = player;

        // partner linkage (bi-directional)
        groundTP.partner   = platformTP.transform;
        platformTP.partner = groundTP.transform;

        // allow Teleport.cs to notify us once the player teleports
        groundTP.director   = this;
        platformTP.director = this;

        // Make sure the trigger areas are centered & are triggers
        var gc = groundPortalGO.GetComponent<BoxCollider2D>();
        var pc = platformPortalGO.GetComponent<BoxCollider2D>();
        if (gc != null) { gc.isTrigger = true; gc.offset = Vector2.zero; }
        if (pc != null) { pc.isTrigger = true; pc.offset = Vector2.zero; }

        return true;
    }

    // Called by Teleport.cs after a successful teleport
    public void NotifyTeleported()
    {
        teleportedOnce = true;
    }

    // Optional getter if other scripts need access
    public Transform GetPlayer() => player;
}
