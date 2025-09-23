using UnityEngine;

public class Enemy_Chaseing : MonoBehaviour
{

    public GameObject player;
    public GameObject self;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDistance = player.transform.position;
        // Debug.Log(playerDistance);

        if (Vector2.Distance(player.transform.position, self.transform.position) < 10)
        {
            Debug.Log("In Range");
        }
        else Debug.Log("Not in Range");
    }

    private void flip()
    {
        Vector3 lS = transform.localScale;
        lS.x *= -1;
        transform.localScale = lS;
    }
}
