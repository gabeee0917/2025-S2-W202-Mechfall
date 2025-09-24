using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup blackPanel;            // CanvasGroup on your BlackPanel (alpha controls fade)
    public TextMeshProUGUI tutorialText;      // TMP text object that shows prompts
    public float fadeDuration = 1.5f;

    [Header("Platform Step")]
    public GameObject platformPrefab;         // Prefab: TutorialPlatform (has PlatformGoal + Outline pulse)
    public Transform platformSpawnPoint;      // Empty where the platform should appear

    [Header("Portals")]
    public GameObject portalPrefab;           // Your animated portal prefab (has Teleport + BoxCollider2D)
    public Transform groundPortalSpawnPoint;  // Empty at ground edge
    public Transform platformPortalSpawnPoint;// Empty at the platform edge

    [Header("Player (optional, can be found by tag)")]
    public Transform player;                  // If left null we’ll find by tag “Player” at runtime

    // --- runtime state ---
    bool pressedA, pressedD;
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

    // =========================
    // Main sequence
    // =========================
    IEnumerator RunTutorial()
    {
        // 1) Fade from black
        yield return StartCoroutine(FadeFromBlack());

        // 2) Welcome
        yield return ShowMessage("Welcome to the tutorial", 1.25f);

        // 3) Move left & right
        yield return ShowMessage("Press A to move Left and D to move Right", 0f);
        yield return WaitForMoveKeys();
        ClearMessage();

        // 4) Jump
        yield return ShowMessage("Press Space to jump", 0f);
        yield return WaitForJump();
        ClearMessage();

        // 5) Spawn practice platform & wait for landing
        SpawnPlatform();
        yield return ShowMessage("Jump on the platform", 0f);
        yield return new WaitUntil(() => goalReached);
        ClearMessage();

        // 6) Spawn portals (platform & ground), flip ground instance inward
        if (!SpawnAndLinkPortals())
        {
            Debug.LogWarning("TutorialDirector: Portal prefab or spawn points not set.");
            yield break;
        }

        // 7) Portal prompt; wait for first teleport
        teleportedOnce = false;
        yield return ShowMessage("Enter a portal to come out of the other one", 0f);
        yield return new WaitUntil(() => teleportedOnce);
        ClearMessage();

        // Done – continue your flow here if you like
        Debug.Log("Tutorial complete!");
    }

    // =========================
    // Small helpers
    // =========================
    IEnumerator FadeFromBlack()
    {
        if (blackPanel != null)
        {
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
        else
        {
            yield return null;
        }
    }

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

    IEnumerator WaitForMoveKeys()
    {
        pressedA = pressedD = false;
        while (!(pressedA && pressedD))
        {
            if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
            if (Input.GetKeyDown(KeyCode.D)) pressedD = true;
            yield return null;
        }
    }

    IEnumerator WaitForJump()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
    }

    // =========================
    // Platform
    // =========================
    void SpawnPlatform()
    {
        if (platformPrefab == null || platformSpawnPoint == null)
        {
            Debug.LogWarning("TutorialDirector: Platform prefab or spawn point not set.");
            return;
        }

        var plat = Instantiate(platformPrefab, platformSpawnPoint.position, Quaternion.identity);

        // Wire PlatformGoal -> director so we know when player landed
        var goal = plat.GetComponentInChildren<PlatformGoal>();
        if (goal != null) goal.director = this;
    }

    // Called by PlatformGoal when player touches the platform
    public void OnGoalReached()
    {
        goalReached = true;
    }

    // =========================
    // Portals
    // =========================
    bool SpawnAndLinkPortals()
    {
        if (portalPrefab == null || groundPortalSpawnPoint == null || platformPortalSpawnPoint == null)
            return false;

        // Spawn both
        var groundPortalGO   = Instantiate(portalPrefab,   groundPortalSpawnPoint.position,   Quaternion.identity);
        var platformPortalGO = Instantiate(portalPrefab, platformPortalSpawnPoint.position, Quaternion.identity);

        // Flip ONLY the ground portal horizontally so it faces inward (Option B)
        var s = groundPortalGO.transform.localScale;
        groundPortalGO.transform.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);

        // Link teleporters both ways
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

        // Make them partners (bi-directional)
        groundTP.partner   = platformTP.transform;
        platformTP.partner = groundTP.transform;

        // Let Teleport know how to notify us on first success
        groundTP.director   = this;
        platformTP.director = this;

        // Optional: ensure both BoxCollider2D are triggers and roughly centered
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

    // Convenience: public getters for Teleport to read if you want the script to be more generic
    public Transform GetPlayer() => player;
}
