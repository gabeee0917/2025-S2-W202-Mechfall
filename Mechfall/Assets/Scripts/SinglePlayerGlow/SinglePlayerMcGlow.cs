using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// In profile page, make it so you can change mc's glow and save using playerprefs.
public class SinglePlayerMcGlow : MonoBehaviour
{
 
    public TMP_Dropdown glowDropdown;


    void Update()
    {
     
    }

    public void SaveGlowChoice()
    {
        

        string GlowColor = glowDropdown.options[glowDropdown.value].text;
        PlayerPrefs.SetString("McGlowColor", GlowColor);


        PlayerPrefs.Save();
    }
}