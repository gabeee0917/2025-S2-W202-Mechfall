using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        questcomplete = false;
    }        

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

  
    public void openLevelCompletePage()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        dummySinglePlayerLives dummy = player.GetComponent<dummySinglePlayerLives>();
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        currentScore += completionAddScore + (1000 * (dummy.lives)) - (int)(25 * timer);
        
        Finalscore = currentScore;
        winpaneltext.text = $"You Scored: \n  {Finalscore} points!\n";
        if (Finalscore > 6000)
        {
            winpaneltext.text += "\nFantastic!";
        }
    }
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

     public IEnumerator delayCompleteLevel()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject player = GameObject.FindWithTag("Player");
        GameObject ingameui = GameObject.FindWithTag("PlayerUI");
                    if (ingameui != null)
                    {
                        Destroy(ingameui);
                    }
                    if (player != null)
                    {
                        Destroy(player);
                    }
                SceneManager.LoadScene("StagePage");

    }

    public void openLevelLosePage()
    {

        LosePanel.SetActive(true);
        Time.timeScale = 0f;
        losepaneltext.text = $"Better luck next time...";
        Finalscore = currentScore;
    }
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