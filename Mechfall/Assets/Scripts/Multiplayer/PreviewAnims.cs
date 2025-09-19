using UnityEngine;
using UnityEngine.UI;
using TMPro;

// In customisation panel, make it so that the preview characters change animations on toggle press and change material of outline on dropdown change.
public class PreviewAnimation : MonoBehaviour
{
    public Toggle toggle;
    public Animator animator;
    public Animator glowa;
    public Renderer glowr;

      public Material noGlow;
    public Material redGlow;
    public Material blueGlow;
    public Material greenGlow;
    public Material yellaGlow;
    public TMP_Dropdown glowDropdown;
    string glowColor = "";

    void Start()
    {
        animator = GetComponent<Animator>();
        glowr = transform.Find("glow")?.GetComponent<Renderer>();
        glowa = transform.Find("glow")?.GetComponent<Animator>();
    }

    void Update()
    {
        if (toggle.isOn)
        {
            animator.SetBool("selected", true);
            glowa.SetBool("selected", true);
            glowColor = glowDropdown.options[glowDropdown.value].text;

            if (glowr != null)
            {
                if (glowColor == "NO GLOW")
                {
                    glowr.material = noGlow;
                }
                else if (glowColor == "RED")
                {
                    glowr.material = redGlow;
                }
                else if (glowColor == "BLUE")
                {
                    glowr.material = blueGlow;
                }
                else if (glowColor == "GREEN")
                {
                    glowr.material = greenGlow;
                }
                else if (glowColor == "YELLOW")
                {
                    glowr.material = yellaGlow;
                }
            }
        }
        else
        {
            animator.SetBool("selected", false);
            glowa.SetBool("selected", false);
        }

        



    }
}