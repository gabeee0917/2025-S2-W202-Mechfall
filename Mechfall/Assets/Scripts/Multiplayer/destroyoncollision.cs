using UnityEngine;
using Photon.Pun;
using System.Collections;

// For attack collisions and interactions in PVP using RPCs 
// While it may seem to collide with Te One's development features, this script being PVP specific required seperate development
public class DestroyOnCollision : MonoBehaviour
{
    public GameObject effectPrefab;
    private PhotonView photonView;



    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;


        if (gameObject.CompareTag("sword") && other.CompareTag("sword"))
        {

            Vector2 contactPoint = other.ClosestPoint(transform.position);
            GameObject effect = PhotonNetwork.Instantiate(effectPrefab.name, contactPoint, Quaternion.identity);

            PhotonView otherPV = other.GetComponent<PhotonView>();
            if (otherPV != null)
            {
                otherPV.RPC("RPC_DisableSword", RpcTarget.AllBuffered);
            }

        }
        else if (other.CompareTag("bullet"))
        {
            if (other.gameObject != null)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            Vector2 contactPoint = other.ClosestPoint(transform.position);
            GameObject effect = PhotonNetwork.Instantiate(effectPrefab.name, contactPoint, Quaternion.identity);
            if (gameObject.CompareTag("bullet"))
            {
                PhotonNetwork.Destroy(gameObject);
            }

        }
        else if (other.CompareTag("Player") && other.transform != transform.parent)
        {
            Vector2 contactPoint = other.ClosestPoint(transform.position);
            GameObject effect = PhotonNetwork.Instantiate(effectPrefab.name, contactPoint, Quaternion.identity);


            PlayerStatus ps = other.GetComponent<PlayerStatus>();
            if (ps != null)
            {
                ps.photonView.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, 5);

            }
            if (gameObject.CompareTag("bullet"))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }


    }

    [PunRPC]
    public void RPC_DisableSword()
    {
        gameObject.SetActive(false);
    }

}

