using System.Collections;
using UnityEngine;
using TMPro;

public class PrintNpcDialogOneLetterAtATime : MonoBehaviour
{
    public TMP_Text npcDialog;
    public float printSpeed = 0.05f;

    private string text2print;
    private Coroutine printCoroutine;

    private void OnEnable()
    {
        if (npcDialog != null)
        {
            // Get the text that was typed into the TMP component
            text2print = npcDialog.text;

            // Clear the text before starting the effect
            npcDialog.text = "";

            printCoroutine = StartCoroutine(ShowTextOneLetterAtATime());
        }
    }

    private IEnumerator ShowTextOneLetterAtATime()
    {
        for (int i = 0; i <= text2print.Length; i++)
        {
            npcDialog.text = text2print.Substring(0, i);
            yield return new WaitForSeconds(printSpeed);
        }
    }

    private void OnDisable()
    {
        if (printCoroutine != null)
        {
            StopCoroutine(printCoroutine);
        }
    }
}
