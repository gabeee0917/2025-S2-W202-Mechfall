using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

// For connecting to server and creating rooms for PVP in the multiplayer panel
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public GameObject multiplayerPanel;
    public GameObject customiseplayerPanel;
    public TMP_Text connectionStatusText;


    private bool enterOfflineAfterDisconnect = false;
    private bool isMultiplayerMode = false;


    public TMP_Text roomListText;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
        if (PhotonNetwork.IsConnected)
        {
            SetMultiplayerUIActive(true);
            LeaveRoom();

        }
        else
        {
            SetMultiplayerUIActive(false);
            LeaveRoom();
        }
        connectionStatusText.text = "";
        connectionStatusText.gameObject.SetActive(false);
    }

    public void PlaySinglePlayer()
    {

        if (PhotonNetwork.IsConnected)
        {
            enterOfflineAfterDisconnect = true;
            PhotonNetwork.Disconnect();
        }
        else
        {
            StartOfflineMode();
        }
    }

    public void ConnectMultiplayer()
    {
        if (!PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            isMultiplayerMode = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {

            string roomName = createInput.text;
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;

            PhotonNetwork.CreateRoom(roomName, roomOptions);

        }
    }

    public override void OnCreatedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        connectionStatusText.text = $"Successfully created {roomName} \n Waiting for Opponent";
        connectionStatusText.gameObject.SetActive(true);
    }
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public void ExitMultiplayer()
    {
        isMultiplayerMode = false;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        SetMultiplayerUIActive(false);
        connectionStatusText.text = "Disconnected from server";
        connectionStatusText.gameObject.SetActive(true);
    }



    private IEnumerator SaveDataOnPvPDC()
    {
        yield return StartCoroutine(UserSession.Instance.SaveDataToFireStore());
    }


    private void StartOfflineMode()
    {
        enterOfflineAfterDisconnect = false;
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("OfflineRoom");
    }

    public override void OnConnectedToMaster()
    {
        connectionStatusText.text = "Connected to server";
        connectionStatusText.gameObject.SetActive(true);
        PhotonNetwork.JoinLobby();

        if (isMultiplayerMode)
        {
            SetMultiplayerUIActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {

            PhotonNetwork.LoadLevel("StagePage");
        }

    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("PVP");

        }
    }

    private void SetMultiplayerUIActive(bool active)
    {
        multiplayerPanel.SetActive(active);

    }

    private void SetCustomiseUIActive(bool active)
    {
        customiseplayerPanel.SetActive(active);
    }

    public void OpenCustPanel()
    {
        SetCustomiseUIActive(true);
    }

    public void CloseCustPanel()
    {
        SetCustomiseUIActive(false);
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            connectionStatusText.text = "Left the room";
        }
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        connectionStatusText.text = "No such room";
        connectionStatusText.gameObject.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomListText.text = "";

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList || !room.IsVisible)
            {
                continue;
            }

            roomListText.text += $"{room.Name}\n";
        }
    }

    public void StoryMode()
    {
            SceneManager.LoadScene("StagePage");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TrainingMap()
    {

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom("TrainingRoom", roomOptions);
        StartCoroutine(WaitForJoinAndLoadScene());

    }

    private IEnumerator WaitForJoinAndLoadScene()
    {
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene("PVP Training Map");
    }

}
