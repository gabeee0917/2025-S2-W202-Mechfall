using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CustomizePage : MonoBehaviour
{
    public Toggle boyToggle;
    public Toggle girlToggle;


    public TMP_Dropdown glowDropdown;

    public TMP_Text descrip;

    void Update()
    {
    if (boyToggle.isOn)
        {
            descrip.text = "[Master Hunter]\nHe will not rest until he has hunted down all his enemies...";
        }
        else if (girlToggle.isOn)
        {
            descrip.text = "[Master Warrior]\nAll she wants is to duel, with whomever it may be...";
        }
        else
        {
            descrip.text = "";
        }    
    }

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