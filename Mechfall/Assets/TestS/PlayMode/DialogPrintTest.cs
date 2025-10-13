using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class DialogPrintTest
{
    private GameObject obj;
    private TMP_Text text;
    private PrintNpcDialogOneLetterAtATime script;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        obj = new GameObject();
        text = obj.AddComponent<TextMeshProUGUI>();
        script = obj.AddComponent<PrintNpcDialogOneLetterAtATime>();

        script.npcDialog = text;
        script.printSpeed = 0.5f; 

        text.text = "Hi"; // Input to be printed
        obj.SetActive(false); // Prevent OnEnable from running immediately
        yield return null;
    }

    [UnityTest]
    public IEnumerator DialogAppearsLetterByLetter()
    {
        obj.SetActive(true); 

        yield return new WaitForSeconds(0.1f); // Before first letter
        Assert.AreEqual("", text.text); // Should be empty

        yield return new WaitForSeconds(0.5f); // First letter should appear
        Assert.AreEqual("H", text.text); // Only H

        yield return new WaitForSeconds(0.5f); // Second letter should appear
        Assert.AreEqual("Hi", text.text); // Now Hi
    }
}
