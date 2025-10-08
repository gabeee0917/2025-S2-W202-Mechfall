using UnityEngine;

public class Storypanel : MonoBehaviour
{
    public GameObject storypanel;

    public void ShowStoryPanel()
    {
        if (storypanel != null)
        {
            storypanel.SetActive(true);
        }
    }
    public void HideStoryPanel()
    {
        if (storypanel != null)
        {
            storypanel.SetActive(false);
        }
    }

}
