using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverStageInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text stageinfo;
    public string onhover;
    public string onleavehover = ""; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (stageinfo != null)
            stageinfo.text = onhover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (stageinfo != null)
            stageinfo.text = onleavehover;
    }
}
