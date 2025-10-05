using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

// Manages both left and right UI (of class UIManagerMulti) in PVP
public class MultiplayerUIManager : MonoBehaviourPunCallbacks
{

    private GameObject[] players;

    public GameObject leftPlayer;
    public PlayerStatus left;
    public GameObject rightPlayer;
    public PlayerStatus right;

   
    public UIManagerMulti UIL;
    public UIManagerMulti UIR;

    public TMP_Text timer;


    public float startTime;
    public float timeGone;

    public TMP_Text bulletcountL;
    public TMP_Text bulletcountR;

    public GunSwordManager leftGS;
    public GunSwordManager rightGS;

    public bool playersallspawned;

    void Start()
    {
        playersallspawned = false;
        StartCoroutine(AssignUI2Players());
    }

    // assigns players to the correct ui positions, room maker spawns on the left and gets left ui, room joiner gets the right, link all componenets of player to the UI
    IEnumerator AssignUI2Players()
    {

        while (players == null || players.Length < 2)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            yield return new WaitForSeconds(0.3f);
        }
        
        if (players[0].transform.position.x < players[1].transform.position.x)
        {
            leftPlayer = players[0];
            rightPlayer = players[1];
        }
        else
        {
            rightPlayer = players[0];
            leftPlayer = players[1];
        }

        left = leftPlayer.GetComponent<PlayerStatus>();
        right = rightPlayer.GetComponent<PlayerStatus>();

        leftGS = leftPlayer.GetComponent<GunSwordManager>();
        rightGS = rightPlayer.GetComponent<GunSwordManager>();


        UIL.playerStatus = left;
        UIR.playerStatus = right;

        SpriteRenderer srL = leftPlayer.GetComponent<SpriteRenderer>();
        if (srL != null && UIL.playerImage != null)
        {
            UIL.playerImage.sprite = srL.sprite;
            UIL.playerImage.material = srL.material;
        }

        SpriteRenderer srR = rightPlayer.GetComponent<SpriteRenderer>();
        if (srR != null && UIR.playerImage != null)
        {
            UIR.playerImage.sprite = srR.sprite;
            UIL.playerImage.material = srR.material;
        }


        startTime = Time.time;
        playersallspawned = true;
    }

    void Awake()
    {

    }

    // update the multiplayer UI, for health and bullets left, countdown from 300, also checks if game is over (players have a bool called gameover that becomes true when they die twice)
    // if left player wins, show left player win screen and right player lose screen and vice versa. 
    // implementation was tricky due to setting the right conditionals regarding which player (which view) causes the gameover event to occur 
    void Update()
    {
        if (leftPlayer != null)
        {


            if (left != null)
            {
                UIL.healthText.text = $"Health: {left.health}";
            }

             if (leftPlayer.GetComponent<PhotonView>().IsMine)
            {
                bulletcountL.text = "Bullets Left: " + leftGS.bullets;
                if (left.gameover)
                {
                    left.ShowLose();
                    left.gameover = false;
                    
                }
                else if (right.gameover)
                {
                    left.ShowWin();
                    left.gameover = false;
                }
            }

        }

        if (rightPlayer != null)
        {

            if (right != null)
            {
                UIR.healthText.text = $"Health: {right.health}";
            }

            if (rightPlayer.GetComponent<PhotonView>().IsMine)
            {
                bulletcountR.text = "Bullets Left: " + rightGS.bullets;
                if (right.gameover)
                {
                    right.ShowLose();
                    right.gameover = false;
                }
                else if (left.gameover)
                {

                    right.ShowWin();
                    right.gameover = false;
                }
            }

        }

       
        
        timeGone = Time.time - startTime;
        timer.text = ((int)(300 - timeGone)).ToString();

    }

    // for leave room button in playroom
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

    }

    // for the situation where one player force closes game or dcs and leaves room, makes remaining player win
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (playersallspawned == true)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                if (leftPlayer != null && leftPlayer.GetComponent<PhotonView>().IsMine)
                {
                    left.ShowWin();
                }
                else if (rightPlayer != null && rightPlayer.GetComponent<PhotonView>().IsMine)
                {
                    right.ShowWin();
                }
            }
        }
    }

    
  
}
