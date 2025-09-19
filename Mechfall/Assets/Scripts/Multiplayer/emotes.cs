using UnityEngine;
using Photon.Pun;

// For displaying emotes in PVP using RPC 
public class emotes : MonoBehaviourPun
{
    public GameObject[] emotearray;
    public GameObject emoteBox;
    private SpriteRenderer emoter;

    public float startTime;
    public float timer;

    void Start()
    {
        emoter = emoteBox.GetComponent<SpriteRenderer>();
        startTime = Time.time;

    }


    void Update()
    {
        if (!photonView.IsMine) return;
        timer = Time.time - startTime;

        if (timer > 2)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                photonView.RPC("RPC_ShowEmote", RpcTarget.All, 0);
                startTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                photonView.RPC("RPC_ShowEmote", RpcTarget.All, 1);
                startTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                photonView.RPC("RPC_ShowEmote", RpcTarget.All, 2);
                startTime = Time.time;
            }
            else
            {
                photonView.RPC("RPC_ShowEmote", RpcTarget.All, 777);
            }
        }



    }
    
    [PunRPC]
    void RPC_ShowEmote(int index)
    {
        if (index == 777)
        {
            emoter.sprite = null;
        }
        else
        {
            emoter.sprite = emotearray[index].GetComponent<SpriteRenderer>().sprite;
        }
            
    }

}
