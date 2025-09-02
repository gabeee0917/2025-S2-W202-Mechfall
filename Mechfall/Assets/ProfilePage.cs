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

    void Awake()
    {
        username.text = "Username: " + UserSession.Instance.username;
        maxlevel.text = "Max Level Reached: " + UserSession.Instance.maxlevel.ToString();
        highscore.text = "High Score: " + UserSession.Instance.score.ToString();
        profilemessage.text = UserSession.Instance.profilemessage;
        
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

    void Update(){
        if( profilemessage.text != UserSession.Instance.profilemessage){
             profilemessage.text = UserSession.Instance.profilemessage;
        }
    }

}