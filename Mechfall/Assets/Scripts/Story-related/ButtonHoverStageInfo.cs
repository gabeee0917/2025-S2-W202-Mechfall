using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// make it so that we can display story text when hovering over a stage button, so that the user is up to date with the story 
public class ButtonHoverStageInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text stageinfo;
    public string onhover;
    public string onleavehover = ""; 

    // shows story text, written inside the editor element for each stage button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (stageinfo != null)
        {
            stageinfo.text = onhover;
        }
    }

    // empty string for unhovering
    public void OnPointerExit(PointerEventData eventData)
    {
        if (stageinfo != null)
        {
            stageinfo.text = onleavehover;
        }
    }
}
