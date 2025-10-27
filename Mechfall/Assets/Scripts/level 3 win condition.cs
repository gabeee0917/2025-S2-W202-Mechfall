using UnityEngine;

public class CoinLevelEnder : MonoBehaviour
{
    [Header("References")]
    public GameObject levelEnder; 

    private float startTime;
    private float timer;
    private float coinCheckTimer;
    private int coinCount;
    private bool levelover;

    private void Start()
    {
        startTime = Time.time;
        levelover = false;

        if (levelEnder != null)
            levelEnder.SetActive(false);
    }

    private void Update()
    {
        
        timer = Time.time - startTime;
        int seconds = (int)timer;

        if (!levelover)
        {
            coinCheckTimer += Time.deltaTime;
            if (coinCheckTimer >= 0.3f)
            {
                coinCount = GameObject.FindGameObjectsWithTag("Coin").Length;

                if (coinCount == 0)
                {
                    //if (levelEnder != null)
                    //levelEnder.SetActive(true);
                    GameObject stagescorecompletemanager = GameObject.FindGameObjectsWithTag("PlayerUI")[0];
                    StageScoreCompleteManager sscm = stagescorecompletemanager.GetComponentInChildren<StageScoreCompleteManager>();
                    if (sscm != null)
                    {
                        sscm.openLevelCompletePage();
                    }
                    levelover = true;
                }

                coinCheckTimer = 0f;
            }
        }
    }
}
