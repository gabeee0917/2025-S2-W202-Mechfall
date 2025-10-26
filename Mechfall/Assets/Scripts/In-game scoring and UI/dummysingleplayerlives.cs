using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.ComponentModel;

//Life system for singleplayer mode player
public class dummySinglePlayerLives : MonoBehaviour
{
    public long lives = 3;
    private float iframeDura = 1f;
    private float iframeTimer = 0f;
    private bool isInvincible = false; 
    public bool panelstopinfinitecall = false;
    private Vector3 startPosition;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        startPosition = transform.position;
        lives = 3;
    }

    void Update()
    {
        //iframe timer
        if (iframeTimer > 0)
        {
            iframeTimer -= Time.deltaTime;
            if (iframeTimer <= 0)
            {
                isInvincible = false;
            }
        }

        //if fall off platform and goes below too much
        if (transform.position.y < -15f)
        {
            lives--;
            transform.position = startPosition;
        }

        if (lives == 0 && panelstopinfinitecall == false)
        {
            GameObject stagescorecompletemanager = GameObject.FindGameObjectsWithTag("PlayerUI")[0];
            StageScoreCompleteManager sscm = stagescorecompletemanager.GetComponentInChildren<StageScoreCompleteManager>();
            if (sscm != null)
            {
                sscm.openLevelLosePage();
                panelstopinfinitecall = true;
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            SoundManager.instance.PlayCollect();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Life"))
        {
            Destroy(other.gameObject);
            lives++;
        }

        if (isInvincible) return;
        if (other.CompareTag("Enemy"))
        {
            lives--;
            isInvincible = true;
            iframeTimer = iframeDura;
        }

        //WIN CONDITION
        if (other.CompareTag("LevelEnder"))
        {
            GameObject stagescorecompletemanager = GameObject.FindGameObjectsWithTag("PlayerUI")[0];
            StageScoreCompleteManager sscm = stagescorecompletemanager.GetComponentInChildren<StageScoreCompleteManager>();
            if (sscm != null)
            {
                sscm.openLevelCompletePage();
                other.gameObject.SetActive(false); // prevent multiple interactions
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            lives--;
            isInvincible = true;
            iframeTimer = iframeDura;
        }
    }
}