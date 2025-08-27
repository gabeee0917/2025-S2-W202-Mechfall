using UnityEngine;
using UnityEngine.UI;
public class SettingsButton : MonoBehaviour
{
    private bool settingsOpen = false;
    public GameObject settingsIcons;

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
}