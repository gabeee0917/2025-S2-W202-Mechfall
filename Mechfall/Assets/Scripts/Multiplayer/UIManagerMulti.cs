using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

// Individual UI links for each side in the PVP map
// Obsolete but useful for tracking and assigning to UI prefab inside unity editor 
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
