using UnityEngine;

public class CoinLevelEnder : MonoBehaviour
{
    [Header("References")]
    public GameObject levelEnder; 

    private float startTime;
    private float timer;
    private float coinCheckTimer;
    private int coinCount;

    private void Start()
    {
        startTime = Time.time;

        if (levelEnder != null)
            levelEnder.SetActive(false);
    }

    private void Update()
    {
        timer = Time.time - startTime;
        int seconds = (int)timer;

        coinCheckTimer += Time.deltaTime;
        if (coinCheckTimer >= 0.3f)
        {
            coinCount = GameObject.FindGameObjectsWithTag("Coin").Length;

            if (coinCount == 0)
            {
                if (levelEnder != null)
                    levelEnder.SetActive(true);
            }

            coinCheckTimer = 0f;
        }
    }
}
