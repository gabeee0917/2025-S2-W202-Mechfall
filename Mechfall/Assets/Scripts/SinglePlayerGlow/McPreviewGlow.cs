using UnityEngine;
using UnityEngine.UI;
using TMPro;

// In profile page, make it so you can change mc's aura/glow and show it in the image (just color change due to UI limitations)
public class McPreviewGlow : MonoBehaviour
{
    public Image glowImage;                 
    public TMP_Dropdown glowDropdown;      
    private string glowColor = "";

    void Start()
    {
        glowImage = GetComponent<Image>();       
    }

    void Update()
    {
        glowColor = glowDropdown.options[glowDropdown.value].text;

        if (glowImage != null)
        {
            switch (glowColor)
            {
                case "NO GLOW":
                    glowImage.color = Color.white; // default
                    break;
                case "RED":
                    glowImage.color = Color.red;
                    break;
                case "BLUE":
                    glowImage.color = Color.blue;
                    break;
                case "GREEN":
                    glowImage.color = Color.green;
                    break;
                case "YELLOW":
                    glowImage.color = Color.yellow;
                    break;
            }
        }
    }
}
