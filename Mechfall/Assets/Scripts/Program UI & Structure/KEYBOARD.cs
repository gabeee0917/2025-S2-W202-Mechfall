using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

// Script for changing color when key pressed on the keymap panel and displaying the in-game function of the key 
public class KeyBoard : MonoBehaviour
{
    public GameObject spacekey;
    public GameObject a;
    public GameObject s;
    public GameObject e;
    public GameObject w;
    public GameObject f;
    public GameObject shift;
    public GameObject left;
    public GameObject right;
    public GameObject esc;
    public GameObject f1;
    public GameObject f2;
    public GameObject f3;
    public GameObject g;
    public GameObject x;
    public GameObject f9;
    private Image escRen;
    private Image spaceRen;
    private Image aRen;
    private Image sRen;
    private Image eRen;
    private Image wRen;
    private Image fRen;
    private Image shiftRen;
    private Image leftRen;
    private Image rightRen;
    private Image f1Ren;
    private Image f2Ren;
    private Image f3Ren;
    private Image gRen;
    private Image xRen;
    private Image f9Ren;
    public TMP_Text explain;

    private void Start()
    {
        spaceRen = spacekey.GetComponent<Image>();
        aRen = a.GetComponent<Image>();
        sRen = s.GetComponent<Image>();
        eRen = e.GetComponent<Image>();
        wRen = w.GetComponent<Image>();
        fRen = f.GetComponent<Image>();
        shiftRen = shift.GetComponent<Image>();
        leftRen = left.GetComponent<Image>();
        rightRen = right.GetComponent<Image>();
        escRen = esc.GetComponent<Image>();
        f1Ren = f1.GetComponent<Image>();
        f2Ren = f2.GetComponent<Image>();
        f3Ren = f3.GetComponent<Image>();
        gRen = g.GetComponent<Image>();
        xRen = x.GetComponent<Image>();
        f9Ren = f9.GetComponent<Image>();
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
            spaceRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.A))
        {
            aRen.color = Color.white;
            explain.text = "A: Sword Attack";
        }
        else
        {
            aRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.S))
        {
            sRen.color = Color.white;
            explain.text = "S: Shoot Laser";
        }
        else
        {
            sRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.E))
        {
            eRen.color = Color.white;
            explain.text = "E: Dash";
        }
        else
        {
            eRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.W))
        {
            wRen.color = Color.white;
        }
        else
        {
            wRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.F))
        {
            fRen.color = Color.white;
            explain.text = "F: Interact";
        }
        else
        {
            fRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftRen.color = Color.white;
            explain.text = "Left Shift: Speed Up";
        }
        else
        {
            shiftRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftRen.color = Color.white;
            explain.text = "Left Arrow: Move Left";
        }
        else
        {
            leftRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rightRen.color = Color.white;
            explain.text = "Right Arrow: Move Right";
        }
        else
        {
            rightRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            escRen.color = Color.white;
            explain.text = "Escape: Pause (StoryMode)";
        }
        else
        {
            escRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            f1Ren.color = Color.white;
            explain.text = "F1: Emote1 (PvP)";
        }
        else
        {
            f1Ren.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.F2))
        {
            f2Ren.color = Color.white;
            explain.text = "F2: Emote2 (PvP)";
        }
        else
        {
            f2Ren.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.F3))
        {
            f3Ren.color = Color.white;
            explain.text = "F3: Emote3 (PvP)";
        }
        else
        {
            f3Ren.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.G))
        {
            gRen.color = Color.white;
            explain.text = "G: Stealth (PvP)";
        }
        else
        {
            gRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.X))
        {
            xRen.color = Color.white;
        }
        else
        {
            xRen.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.F9))
        {
            f9Ren.color = Color.white;
            explain.text = "F9: Screenshot";
        }
        else
        {
            f9Ren.color = Color.grey;
        }
    }
}
