using UnityEngine;
using UnityEngine.UI;

//behavior script to show the panel of subbuttons when pressing the gear button (settings) at the top right
public class SettingsButton : MonoBehaviour
{
    private bool settingsOpen = false;
    public GameObject settingsIcons;
    public GameObject keymap;

    public void clickSettings()
    {
        if (settingsOpen == false)
        {
            settingsIcons.SetActive(true);
            settingsOpen = true;
        }
        else if (settingsOpen == true)
        {
            settingsIcons.SetActive(false);
            settingsOpen = false;
        }
    }

    public void closePanel()
    {
        settingsIcons.SetActive(false);
    }
    
    public void openPanel()
    {
        settingsIcons.SetActive(true);
    }

    public void openKeyMap()
    {
        keymap.SetActive(true);
    }

    public void closeKeyMap()
    {
        keymap.SetActive(false);
    }
}