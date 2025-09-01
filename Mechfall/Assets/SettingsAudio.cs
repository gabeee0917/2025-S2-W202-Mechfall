using UnityEngine;
using UnityEngine.UI;


// Behavior script on pressing the sound button at the top right (appears when pressing the gear button). 
//AS OF 02.09.25 HAVE NOT YET ADDED ACTUAL CHANGES TO SOUND, ONLY THE APPEARANCE OF THE AUDIO BUTTON. DONT FORGET TO ADD AND NEED TO DYNAMICALLY GET IF SOUND ON OR NOT!
//dONT JUST SET TO TRUE AT THE BEGINNING!   
public class SettingsAudioButton : MonoBehaviour
{
    private bool Sound = true;
    public GameObject audioOn;
    public GameObject audioOff;

    public void clickAudio()
    {
        if (Sound == true)
        {
            audioOn.SetActive(false);
            audioOff.SetActive(true);
            Sound = false;
        }
        else if (Sound == false)
        {
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            Sound = true;
        }
    }
}