using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// in game player spawner for PVP, uses Player Prefs set out in customisation panel to instantiate the correct player prefab.
// spawnpoint 1 is left side which spawns the room creator (figured out through a few tests) and 2 spawns the joiner
public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
 

    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    private Vector3 spawnPosition;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            string selectedCharacter = PlayerPrefs.GetString("Character", "Girl");
            GameObject playerPrefab;
            if (selectedCharacter == "Boy")
            {
                playerPrefab = boyPrefab;
            }
            else if (selectedCharacter == "Girl")
            {
                playerPrefab = girlPrefab;
            }
            else
            {
                playerPrefab = girlPrefab;
            }

            string selectedGlowColor = PlayerPrefs.GetString("GlowColor", "NO GLOW");



            int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;


            if (spawnIndex == 1)
            {
                spawnPosition = spawnPoint1.transform.position;
            }
            else if (spawnIndex == 2)
            {
                spawnPosition = spawnPoint2.transform.position;
            }

            object[] instantiationData = new object[] { selectedCharacter, selectedGlowColor };

            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0, instantiationData);


        }
        
    }


   public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
