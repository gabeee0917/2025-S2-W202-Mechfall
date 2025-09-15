using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 
using System.Collections.Generic;

public class StageManager: MonoBehaviour
{
    public Dictionary<string, GameObject> stageButtons = new Dictionary<string, GameObject>();

    void Start(){
         Button[] allButtons = GameObject.FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        { 
            if (button.gameObject.name == "Lobby" || button.gameObject.name == "logout"
            ||button.gameObject.name == "profile" || button.gameObject.name == "Settings Button"
            || button.gameObject.name == "Save" || button.gameObject.name == "keymap" 
            || button.gameObject.name == "sound" || button.gameObject.name == "Highscores") 
            {
                continue;
            }

            

            if (!stageButtons.ContainsKey(button.gameObject.name))
            {
                stageButtons.Add(button.gameObject.name, button.gameObject);
            }
        }

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

    void Update(){
      
    }

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

