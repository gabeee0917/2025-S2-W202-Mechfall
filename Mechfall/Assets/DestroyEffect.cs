using UnityEngine;
using Photon.Pun;
using System.Collections;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyDelay());
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.2f);
        PhotonView pv = GetComponent<PhotonView>();
        if (pv != null && pv.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
