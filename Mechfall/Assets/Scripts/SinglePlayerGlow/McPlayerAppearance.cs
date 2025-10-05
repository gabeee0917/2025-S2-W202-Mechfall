
using UnityEngine;


// attach to each player from level 2 onwards 
public class McPlayerAppearance : MonoBehaviour
{
    public Material noGlow;
    public Material redGlow;
    public Material blueGlow;
    public Material greenGlow;
    public Material yellaGlow;

    public SpriteRenderer glowr;
   

    private Color baseGlowColor;
    void Awake()
    {
        glowr = transform.GetComponent<SpriteRenderer>();
  
    }

    void Start()
    {

            string glowColor = PlayerPrefs.GetString("McGlowColor", "NO GLOW");

            if (glowr != null)
            {
                if (glowColor == "NO GLOW")
                {
                    glowr.material = new Material(noGlow);
                }
                else if (glowColor == "RED")
                {
                    glowr.material = new Material(redGlow);
                }
                else if (glowColor == "BLUE")
                {
                    glowr.material = new Material(blueGlow);
                }
                else if (glowColor == "GREEN")
                {
                    glowr.material = new Material(greenGlow);
                }
                else if (glowColor == "YELLOW")
                {
                    glowr.material = new Material(yellaGlow);
                }
            }


    }

}
