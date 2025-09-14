using UnityEngine;
using Photon.Pun;
using System.Collections;

// For destroying the collision effect that is instantiated when sword or laser bullet hits in PVP
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
