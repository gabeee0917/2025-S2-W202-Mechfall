using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
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

    public GameObject esc;

    private Image escRen;
    private Image spaceRen;
    private Image aRen;
    private Image sRen;
    private Image dRen;
    private Image wRen;
    private Image upRen;
    private Image downRen;
    private Image leftRen;
    private Image rightRen;
    public TMP_Text explain;

    private void Start()
    {
        spaceRen = spacekey.GetComponent<Image>();
        aRen = a.GetComponent<Image>();
        sRen = s.GetComponent<Image>();
        dRen = d.GetComponent<Image>();
        wRen = w.GetComponent<Image>();
        upRen = up.GetComponent<Image>();
        downRen = down.GetComponent<Image>();
        leftRen = left.GetComponent<Image>();
        rightRen = right.GetComponent<Image>();
        escRen = esc.GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceRen.color = Color.white;
            explain.text = "SPACE: Jump";
        }
        else
        {
            spaceRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.A))
        {
            aRen.color = Color.white;
            explain.text = "A: Sword Attack";
        }
        else
        {
            aRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.S))
        {
            sRen.color = Color.white;
            explain.text = "S: Shoot Laser";
        }
        else
        {
            sRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.D))
        {
            dRen.color = Color.white;
        }
        else
        {
            dRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.W))
        {
            wRen.color = Color.white;
        }
        else
        {
            wRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            upRen.color = Color.white;
            explain.text = "Up: Interact";
        }
        else
        {
            upRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            downRen.color = Color.white;
        }
        else
        {
            downRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftRen.color = Color.white;
            explain.text = "Left Arrow: Move Left";
        }
        else
        {
            leftRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rightRen.color = Color.white;
            explain.text = "Right Arrow: Move Right";
        }
        else
        {
            rightRen.color = Color.black;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            escRen.color = Color.white;
            explain.text = "Escape: Pause";
        }
        else
        {
            escRen.color = Color.black;
        }
    }

   
    

}
