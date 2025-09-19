using UnityEngine;
using Photon.Pun;

// just a helper script for protecting my eyes in testing out pvp, used ai for specific details
// trying to look at the character with no camera movement hurts my eyes and made me dizzy
// might replace with camera moving code Gabe developed 
public class camerapvpmove : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}
