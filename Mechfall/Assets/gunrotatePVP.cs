using UnityEngine;


public class GunFollowOpp : MonoBehaviour
{
    public Transform target;
    private Transform myPlayer;

    void Start()
    {
        myPlayer = transform.root;

    }

    

    void Update()
    {
        if (target == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {

                if (player.transform == myPlayer)
                {
                    continue;
                }
                target = player.transform;
                break;
            }
        }
        else
        {
            Vector3 direction = target.position - transform.position;
            if (direction.x < 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -180;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                
            }
        }
    }
}
