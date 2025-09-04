using UnityEngine;
using System.Collections;
using TMPro;
public class KeyBoard : MonoBehaviour
{
    public GameObject spacekey;
    public GameObject a;
    public GameObject s;
    public GameObject d;
    public GameObject w;
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;

    public TMP_Text explain;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spacekey.SetActive(true);
            explain.text = "SPACE: Jump";
        }
        else
        {
            spacekey.SetActive(false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            a.SetActive(true);
            explain.text = "A: Sword Attack";
        }
        else
        {
            a.SetActive(false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            s.SetActive(true);
            explain.text = "S: Shoot Laser";
        }
        else
        {
            s.SetActive(false);
        }

         if (Input.GetKey(KeyCode.D))
        {
            d.SetActive(true);
        }
        else
        {
            d.SetActive(false);
        }

         if (Input.GetKey(KeyCode.W))
        {
            w.SetActive(true);
        }
        else
        {
            w.SetActive(false);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            up.SetActive(true);
            explain.text = "Up: Interact";
        }
        else
        {
            up.SetActive(false);
        }

          if (Input.GetKey(KeyCode.DownArrow))
        {
            down.SetActive(true);
        }
        else
        {
            down.SetActive(false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            left.SetActive(true);
            explain.text = "Left Arrow: Move Left";
        }
        else
        {
            left.SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            right.SetActive(true);
            explain.text = "Right Arrow: Move Right";
        }
        else
        {
            right.SetActive(false);
        }

    }

   
    

}
