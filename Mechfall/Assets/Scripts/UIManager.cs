using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// replace popuppanel with pause menu when vidu uploads,  
public class UIManager : MonoBehaviour
{
     public TextMeshProUGUI LivesText;
    public Image playerImage;

   
    public dummySinglePlayerLives dummy;
    public GameObject popupPanel;
    public GameObject player;

    public bool ispaused = false; 

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

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

         if (InputManager.pausePressed)
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

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        ispaused = true;
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        ispaused = false;
    }

    public void ToLobby(){
        SceneManager.LoadScene("Lobby");
        Time.timeScale = 1f;

        if (player != null)
        {
            Destroy(player);
        }

        Destroy(gameObject);
    }

    public void ToStage(){
        SceneManager.LoadScene("StagePage");
        Time.timeScale = 1f;

        if (player != null)
        {
            Destroy(player);
        }
        
        Destroy(gameObject);
    }
}
