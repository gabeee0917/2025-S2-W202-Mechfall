using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
using System.Collections;

// Script is in UIManager prefab that is in every single player level 
public class StageScoreCompleteManager : MonoBehaviour
{
    public enum QuestType { CollectCoins, ActivateSwitches, DefeatBoss }

    [Header("References")]
    public GameObject LevelEnder;
    public GameObject player;

    [Header("UI References")]
    public TMP_Text timeUI;
    public TMP_Text questText;
    public TMP_Text winpaneltext;
    public TMP_Text losepaneltext;
    public GameObject WinPanel;
    public GameObject LosePanel;

    [Header("Quest Settings")]
    public QuestType questType;
    public int completionScore = 1000;
    public int totalSwitches = 3;

    private int activatedSwitches = 0;
    private int remainingCoins;
    private bool questComplete = false;
    private float coinCheckTimer = 0f;

    [Header("Scoring & Timing")]
    public long currentScore;
    public long Finalscore;
    private float startTime;
    private float timer;
    public int currentLevelNum;
    public string currentLevelName;

    // on awake when scene loads, initialise the time, score, scenename, quest text, and corresponding UI elements
    public void initializeLevel()
    {
        startTime = Time.time;
        currentScore = 0;
        questComplete = false;
        if (LevelEnder) LevelEnder.SetActive(false);

        switch(questType)
        {
            case QuestType.CollectCoins:
                remainingCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
                UpdateQuestText($"Collect all crystals!\nRemaining: {remainingCoins}");
                break;

            case QuestType.ActivateSwitches:
                totalSwitches = GameObject.FindGameObjectsWithTag("Switch").Length;
                activatedSwitches = 0;
                UpdateQuestText($"Activate {totalSwitches} switches!");
                break;

            case QuestType.DefeatBoss:
                UpdateQuestText("Defeat the boss!");
                break;
        }
    }
    void Awake()
    {
        currentLevelName = SceneManager.GetActiveScene().name;
        int.TryParse(currentLevelName, out currentLevelNum);
        initializeLevel();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // had to do this so that each partial level 1-1. 1-2 etc can have seperate crystal collection quests, all of them adding on to the final score
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        questComplete = false;
    }
    
    // Level 2 Switch Quest 
    public void SwitchActivated()
    {
        if (questType != QuestType.ActivateSwitches) return;

        activatedSwitches++;
        SoundManager.instance.PlayCapture();
        int remaining = Mathf.Max(totalSwitches - activatedSwitches, 0);
        UpdateQuestText($"Switches remaining: {remaining}");

        if (activatedSwitches >= totalSwitches) CompleteQuest();
    }

    // updating the in game UI for time, number of crystals in the map 
    public void Update()
    {
        UpdateTimerUI();

        if (questType == QuestType.CollectCoins && !questComplete)
        {
            coinCheckTimer += Time.deltaTime;
            if (coinCheckTimer >= 0.3f)
            {
                remainingCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
                UpdateQuestText($"Remaining crystals: {remainingCoins}");
                if (remainingCoins <= 0) CompleteQuest();
                coinCheckTimer = 0f;
            }
        }
    }

    public void UpdateTimerUI()
    {
        timer = Time.time - startTime;
        int seconds = Mathf.FloorToInt(timer);
        timeUI.text = seconds.ToString();
    }

    public void UpdateQuestText(string message)
    {
        if (questText) questText.text = message;
    }

    public void CompleteQuest()
    {
        questComplete = true;
        currentScore += completionScore;
        UpdateQuestText("Quest complete!");
        if (LevelEnder) LevelEnder.SetActive(true);
    }

    // open level complete panel when player reaches the level ender portal
    public void openLevelCompletePage()
    {
        dummySinglePlayerLives dummy = player.GetComponent<dummySinglePlayerLives>();
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        currentScore += completionScore + (1000 * (dummy.lives)) - (int)(2 * timer);

        Finalscore = currentScore;
        winpaneltext.text = $"You Scored: \n  {Finalscore} points!\n";
        if (Finalscore > 6000)
        {
            winpaneltext.text += "\nFantastic!";
        }
    }

    // close level complete page when the user presses a button on the page 
    public void closeLevelCompletePage()
    {
        if (currentScore > UserSession.Instance.levelscores[currentLevelNum - 1])
        {
            UserSession.Instance.levelscores[currentLevelNum - 1] = Finalscore;
            UserSession.Instance.updateHighScore();
        }
        if (currentLevelNum == UserSession.Instance.maxlevel)
        {
            UserSession.Instance.maxlevel = currentLevelNum + 1;
        }
        UserSession.Instance.saveB();
        //WinPanel.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(delayCompleteLevel());
    }

    //delayed completion to ensure proper destruction of dontdestroyonload objects
    public IEnumerator delayCompleteLevel()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(player);
        GameObject ingameui = GameObject.FindWithTag("PlayerUI");
        if (ingameui != null)
        {
            Destroy(ingameui);
        }

        GameObject[] sounds = GameObject.FindGameObjectsWithTag("Sound");
        foreach (GameObject sound in sounds)
        {
            if (sound != null)
            {
                Destroy(sound);
            }
        }

        GameObject soundmanager = GameObject.FindWithTag("Spawn");

        if (soundmanager != null)
        {
            Destroy(soundmanager);
        }

        SceneManager.LoadScene("StagePage");
    }

    // open lose panel when player lives reaches 0
    public void openLevelLosePage()
    {
        LosePanel.SetActive(true);
        Time.timeScale = 0f;
        losepaneltext.text = $"Better luck next time...";
        Finalscore = currentScore;
    }

    // close lose panel when press button
    public void closeLevelLosePage()
    {
        if (currentScore > UserSession.Instance.levelscores[currentLevelNum - 1])
        {
            UserSession.Instance.levelscores[currentLevelNum - 1] = Finalscore;
            UserSession.Instance.updateHighScore();
        }
        UserSession.Instance.saveB();
        Time.timeScale = 1f;
        StartCoroutine(delayCompleteLevel());
    }
}