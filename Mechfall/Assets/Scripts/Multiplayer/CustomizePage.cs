using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Customisation panel, uses Player Prefs to store character prefab to be spawned and the glow appearance settings locally 
public class CustomizePage : MonoBehaviour
{
    public Toggle boyToggle;
    public Toggle girlToggle;
    public TMP_Dropdown glowDropdown;
    public TMP_Text descrip;

    // updates the description text of the character chosen (toggled)
    void Update()
    {
        if (boyToggle.isOn)
        {
            descrip.text = "[DemonFall]\nChosen by the light in the 18th century, he hunted down every last demon and saved humanity from the darkness...";
        }
        else if (girlToggle.isOn)
        {
            descrip.text = "[DragonFall]\nChosen by the light in the 14th century, she slayed every last dragon and saved humanity from the fire...";
        }
        else
        {
            descrip.text = "";
        }
    }

    // Done button, saves the character choice and the glow color in pvp
    public void SaveCharacterChoice()
    {
        string Character;
        if (boyToggle.isOn)
        {
            Character = "Boy";
        }
        else if (girlToggle.isOn)
        {
            Character = "Girl";
        }
        else
        {
            Character = "Girl";
        }
        PlayerPrefs.SetString("Character", Character);

        string GlowColor = glowDropdown.options[glowDropdown.value].text;
        PlayerPrefs.SetString("GlowColor", GlowColor);

        PlayerPrefs.Save();
    }
}