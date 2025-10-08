using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

// Script is in UIManager prefab that is in every single player level 
public class StageScoreCompleteManager : MonoBehaviour
{
    public string currentLevelName;

    public long currentScore;
    public long Finalscore;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public int currentLevelNum;


    public int completionAddScore = 1000;

    public float startTime;
    public float timer;
    public TMP_Text timeUI;
    public TMP_Text winpaneltext;
    public TMP_Text losepaneltext;


    public bool questyes;
    public TMP_Text collectcoinquesttext;
    float coinCheckTimer = 0f;
    public long coincount;

    public bool questcomplete = false;

    public GameObject player;

    // on awake when scene loads, initialise the time, score, scenename, quest text, and corresponding UI elements
    void Awake()
    {
        currentLevelName = SceneManager.GetActiveScene().name;
        currentScore = 0;
        currentLevelNum = int.Parse(currentLevelName);
        startTime = Time.time;
        timeUI.text = startTime.ToString();
        if (questyes)
        {
            coincount = GameObject.FindGameObjectsWithTag("Coin").Length;
            collectcoinquesttext.text = "Remaining crystals in map: " + coincount.ToString();
        }
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
        questcomplete = false;
    }
    

    // updating the in game UI for time, number of crystals in the map 
    void Update()
    {


        timer = Time.time - startTime;
        int seconds = (int)timer;
        timeUI.text = seconds.ToString();

        if (questyes && questcomplete == false)
        {
            coinCheckTimer += Time.deltaTime;
            if (coinCheckTimer >= 0.3f)
            {
                coincount = GameObject.FindGameObjectsWithTag("Coin").Length;
                collectcoinquesttext.text = "Remaining crystals in map: " + coincount.ToString();
                if (coincount == 0)
                {
                    collectcoinquesttext.text = "Found all crystals!";

                    questcomplete = true;
                    currentScore += 1000;
                }
                coinCheckTimer = 0f;
            }
        }

    }

    public void AddScore(int n)
    {
        currentScore += n;
    }

    public void SubScore(int n)
    {
        currentScore -= n;
    }


    // open level complete panel when player reaches the level ender portal
    public void openLevelCompletePage()
    {

        dummySinglePlayerLives dummy = player.GetComponent<dummySinglePlayerLives>();
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        currentScore += completionAddScore + (1000 * (dummy.lives)) - (int)(2 * timer);

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
        //LosePanel.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(delayCompleteLevel());

    }
}