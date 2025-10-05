using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// single player In-game UI manager, links multiple aspects together
public class UIManager : MonoBehaviour
{
     public TextMeshProUGUI LivesText;
    public Image playerImage;

   
    public dummySinglePlayerLives dummy;
    public GameObject popupPanel;
    public GameObject player;

    public bool ispaused = false;

    // on start, make this dontdestroyonload so that it persists across sublevels 1-1, 1-2 etc, initialise UI elements
    void Start()
    {
        DontDestroyOnLoad(gameObject);
         if (player != null)
            {
                dummy = player.GetComponent<dummySinglePlayerLives>();

                SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
                if (sr != null && playerImage != null)
                {
                    playerImage.sprite = sr.sprite;
                }
            }
    }

    // if uninitialised at start, attempt to find player, update lives, check for ESC input for pause menu
    void Update()
    {
        if (dummy == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                dummy = player.GetComponent<dummySinglePlayerLives>();

                SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
                if (sr != null && playerImage != null)
                {
                    playerImage.sprite = sr.sprite;
                }
            }
        }

        if (dummy != null)
        {
            LivesText.text = $"Lives: {dummy.lives}";
        }

         if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ispaused == false)
                {
                    ShowPopup();
                }
                else
                {
                    HidePopup();
                }
            }
    }

    //pause menu popup, freeze game
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
    }

    //pause menu hide, unfreeze game, resume button in pause menu
    public void HidePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
    }

    // to lobby button in pause menu
    public void ToLobby()
    {
        SceneManager.LoadScene("Lobby");
        Time.timeScale = 1f;

        if (player != null)
        {
            Destroy(player);
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

        Destroy(gameObject);
    }

    // to stagepage button in pause menu
    public void ToStage()
    {
        SceneManager.LoadScene("StagePage");
        Time.timeScale = 1f;

        if (player != null)
        {
            Destroy(player);
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

        Destroy(gameObject);
    }
}
