using UnityEngine;
using UnityEngine.UI;

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