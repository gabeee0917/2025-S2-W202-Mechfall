using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; 
using TMPro;
using System.Collections;

public class Profilepage : MonoBehaviour
{
    public TMP_Text username;
    public TMP_Text maxlevel;

    public TMP_Text highscore;

    public TMP_Text profilemessage;

    public TMP_InputField changeprofilemessage;

    private string previousmessage;

    public TMP_Text PvPWin;
    public TMP_Text PvPLose;

    void Awake()
    {
        username.text = UserSession.Instance.username;
        maxlevel.text = UserSession.Instance.maxlevel.ToString();
        highscore.text = UserSession.Instance.score.ToString();
        profilemessage.text = UserSession.Instance.profilemessage;
        PvPWin.text = UserSession.Instance.PvPWin.ToString();
        PvPLose.text = UserSession.Instance.PvPLose.ToString();
    }

    public void ChangeMessage() {
        profilemessage.text = changeprofilemessage.text;
        UserSession.Instance.profilemessage = changeprofilemessage.text;
        UserSession.Instance.saveB();
        hideInputfield();  
    }

    public void showInputfield() {
        changeprofilemessage.gameObject.SetActive(true);
    }

    public void hideInputfield() {
        changeprofilemessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (profilemessage.text != UserSession.Instance.profilemessage)
        {
            profilemessage.text = UserSession.Instance.profilemessage;
        }
        if (maxlevel.text != UserSession.Instance.maxlevel.ToString())
        {
            maxlevel.text = UserSession.Instance.maxlevel.ToString();
        }
        if (highscore.text != UserSession.Instance.score.ToString())
        {
            highscore.text = UserSession.Instance.score.ToString();
        }
        if (PvPWin.text != UserSession.Instance.PvPWin.ToString())
        {
            PvPWin.text = UserSession.Instance.PvPWin.ToString();
        }
        if (PvPLose.text != UserSession.Instance.PvPLose.ToString())
        {
            PvPLose.text = UserSession.Instance.PvPLose.ToString();
        }
    }

}