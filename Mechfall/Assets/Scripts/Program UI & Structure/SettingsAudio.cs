using UnityEngine;
using UnityEngine.UI;


// Behavior script on pressing the sound button at the top right (appears when pressing the gear button). 
 
public class SettingsAudioButton : MonoBehaviour
{
    private bool Sound = true;
    public GameObject audioOn;
    public GameObject audioOff;

    // if audio is on, turn off audio and change icon to off icon. if audio is off, turn on audio and change icon to on icon
    public void clickAudio()
    {
        if (Sound == true)
        {
            audioOn.SetActive(false);
            audioOff.SetActive(true);
            Sound = false;
            AudioListener.volume = 0f;
        }
        else if (Sound == false)
        {
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            Sound = true;
            AudioListener.volume = 1f;
        }
    }

    // synchs the button display with correct sound state (has become obsolete as the idea of a sound button in pause menu was scrapped)
    public void Update()
    {
        if (AudioListener.volume > 0f)
        {
            if (Sound == false)
            {
                audioOn.SetActive(true);
                audioOff.SetActive(false);
                Sound = true;
            }
        }
        else
        {
            if (Sound == true)
            {
                audioOn.SetActive(false);
                audioOff.SetActive(true);
                Sound = false;
            }
        }
    }
    }