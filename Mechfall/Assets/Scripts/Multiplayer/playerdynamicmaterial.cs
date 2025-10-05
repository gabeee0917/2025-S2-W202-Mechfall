using Photon.Pun;
using UnityEngine;


// when the players are instantiated in PVP map, change the glow of outline, wing, sword, and muzz (muzz only applies to girl character, is actually the light gun)
// to the settings saved in customisation panel Player prefs
public class PlayerAppearance : MonoBehaviourPun
{
    public Material noGlow;
    public Material redGlow;
    public Material blueGlow;
    public Material greenGlow;
    public Material yellaGlow;

    public Renderer glowr;
    public Renderer sword;

    public Renderer wing;
    public Renderer muzz;

    private Color baseGlowColor;
    void Awake()
    {
        glowr = transform.Find("glow")?.GetComponent<Renderer>();
        sword = transform.Find("bladehitbox")?.GetComponent<Renderer>();
        wing = transform.Find("wing")?.GetComponent<Renderer>();
        muzz = transform.Find("muzzflash")?.GetComponent<Renderer>();
    }

    void Start()
    {

        object[] data = photonView.InstantiationData;   // index 0 is character prefab choice, index 1 is glow color choice, done through prefabs 

        if (data != null && data.Length >= 2)
        {
            string glowColor = (string)data[1];

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

            if (sword != null)
            {
                if (glowColor == "NO GLOW")
                {
                    sword.material = new Material(noGlow);
                    //transform.Find("bladehitbox").GetChild(0).GetComponent<Renderer>().material = new Material(noGlow);
                }
                else if (glowColor == "RED")
                {
                    sword.material = new Material(redGlow);
                    //transform.Find("bladehitbox").GetChild(0).GetComponent<Renderer>().material = new Material(redGlow);
                }
                else if (glowColor == "BLUE")
                {
                    sword.material = new Material(blueGlow);
                    //transform.Find("bladehitbox").GetChild(0).GetComponent<Renderer>().material = new Material(blueGlow);
                }
                else if (glowColor == "GREEN")
                {
                    sword.material = new Material(greenGlow);
                    //transform.Find("bladehitbox").GetChild(0).GetComponent<Renderer>().material = new Material(greenGlow);
                }
                else if (glowColor == "YELLOW")
                {
                    sword.material = new Material(yellaGlow);
                    //transform.Find("bladehitbox").GetChild(0).GetComponent<Renderer>().material = new Material(yellaGlow);
                }
            }

            if (muzz != null)
            {
                if (glowColor == "NO GLOW")
                {
                    muzz.material = new Material(noGlow);

                }
                else if (glowColor == "RED")
                {
                    muzz.material = new Material(redGlow);

                }
                else if (glowColor == "BLUE")
                {
                    muzz.material = new Material(blueGlow);

                }
                else if (glowColor == "GREEN")
                {
                    muzz.material = new Material(greenGlow);

                }
                else if (glowColor == "YELLOW")
                {
                    muzz.material = new Material(yellaGlow);

                }
            }

            if (wing != null)
            {
                if (glowColor == "NO GLOW")
                {
                    wing.material = new Material(noGlow);
                }
                else if (glowColor == "RED")
                {
                    wing.material = new Material(redGlow);
                    baseGlowColor = redGlow.GetColor("_GlowColor");
                    wing.material.SetColor("_GlowColor", baseGlowColor * 1.5f);
                }
                else if (glowColor == "BLUE")
                {
                    wing.material = new Material(blueGlow);
                    baseGlowColor = blueGlow.GetColor("_GlowColor");
                    wing.material.SetColor("_GlowColor", baseGlowColor * 1.5f);
                }
                else if (glowColor == "GREEN")
                {
                    wing.material = new Material(greenGlow);
                    baseGlowColor = greenGlow.GetColor("_GlowColor");
                    wing.material.SetColor("_GlowColor", baseGlowColor * 1.5f);
                }
                else if (glowColor == "YELLOW")
                {
                    wing.material = new Material(yellaGlow);
                    baseGlowColor = yellaGlow.GetColor("_GlowColor");
                    wing.material.SetColor("_GlowColor", baseGlowColor * 1.5f);
                }
            }

        }


    }

}
