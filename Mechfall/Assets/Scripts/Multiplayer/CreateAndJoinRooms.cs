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

    // makes it so that when a player plays pvp and leaves room, the panel isnt shut (default for the scene) but open if they are still connected to the network server
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

    // obsolete method, originally attempted implementing single player using photon offline mode but team decided against it
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

    // connect to multiplayer network by pressing PVP button in lobby
    public void ConnectMultiplayer()
    {
        if (!PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            isMultiplayerMode = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //Create a room with the title input, if none input a string of stuff is the room name
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

    // when room is created, changes the status text
    public override void OnCreatedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        connectionStatusText.text = $"Successfully created {roomName} \n Waiting for Opponent";
        connectionStatusText.gameObject.SetActive(true);
    }

    // on room join, change the status text, not really needed as join causes scene change immediately
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    // exiting multiplayer server through the button Leave server
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


    // if pvp dc occurs, incur a loss
    private IEnumerator SaveDataOnPvPDC()
    {
        yield return StartCoroutine(UserSession.Instance.SaveDataToFireStore());
    }

    // obsolete function, for photon offline mode as mentioned before is no longer used
    private void StartOfflineMode()
    {
        enterOfflineAfterDisconnect = false;
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("OfflineRoom");
    }

    // when connect to server, change status text and joins lobby
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

    // obsolete function related to offline mode
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.LoadLevel("StagePage");
        }
    }

    // this is for when a pvp room starts, make the room closed and invisible so that it is no longer on the list, load pvp map
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
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

    // used when leaving room in pvp lobby ui or when pvp match is done
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

    // update the room list with all the visible and open rooms in the photon lobby
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
    
    // for the button single player    
    public void StoryMode()
    {
        SceneManager.LoadScene("StagePage");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // a one player room, more for development purposes to test pvp room but kept so users can play around
    public void TrainingMap()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom("TrainingRoom", roomOptions);
        StartCoroutine(WaitForJoinAndLoadScene());
    }

    //make sure scene load doesn't happen too quickly causing bugs
    private IEnumerator WaitForJoinAndLoadScene()
    {
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene("PVP Training Map");
    }

}
