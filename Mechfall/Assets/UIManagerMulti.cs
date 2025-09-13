using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class UIManagerMulti : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI healthText;

    public TextMeshProUGUI LivesText;
    public Image backgroundImage;
    public Image playerImage;

    //private playerstatus
    public PlayerStatus playerStatus;

    public GameObject player;



   

    void Start()
    {

    }

    void Update()
    {


        if (playerStatus != null)
        {
            healthText.text = $"Health: {playerStatus.health}";
            LivesText.text = $"Lives: {playerStatus.lives}";
        }


    }

 


}
