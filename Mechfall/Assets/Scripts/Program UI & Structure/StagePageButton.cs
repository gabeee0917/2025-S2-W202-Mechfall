using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 
using System.Collections.Generic;

// When the user presses single player, they come to stagepage where it shows the stages / levels they can enter (depending on thier maxlevel data in usersession)
public class StageManager : MonoBehaviour
{
    public Dictionary<string, GameObject> stageButtons = new Dictionary<string, GameObject>();

    // uses a dictionary, find all the buttons that aren't UI related (the stage buttons) and stores them in dictionary with button name (same as level) as key and button object as value
    // on retrospective, an array may have been simpler but we hadnt decided on the number of levels so this implementation wasn't limited by that 
    void Start()
    {
        Button[] allButtons = GameObject.FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        {
            if (button.gameObject.name == "Lobby" || button.gameObject.name == "logout"
            || button.gameObject.name == "profile" || button.gameObject.name == "Settings Button"
            || button.gameObject.name == "Save" || button.gameObject.name == "keymap"
            || button.gameObject.name == "sound" || button.gameObject.name == "Highscores"
            || button.gameObject.name == "Story"|| button.gameObject.name == "Image")
            {
                continue;
            }



            if (!stageButtons.ContainsKey(button.gameObject.name))
            {
                stageButtons.Add(button.gameObject.name, button.gameObject);
            }
        }

        // parse key to int so that it can be compared to max level reached by player to show only levels equal to or below that
        foreach (var kvp in stageButtons)
        {

            if (int.Parse(kvp.Key) <= UserSession.Instance.maxlevel)
            {
                kvp.Value.gameObject.SetActive(true);
            }
            else
            {
                kvp.Value.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {

    }

    // on clicking a stage button, enter that scene. The scene itself is named the level number, as is the buttonname, allowing this to be used as is for each level.
    public void EnterStage()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        if (clickedButton != null)
        {
            string buttonName = clickedButton.name;
            SceneManager.LoadScene(buttonName);

        }
    }


}

