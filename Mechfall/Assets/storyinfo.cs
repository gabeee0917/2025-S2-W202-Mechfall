using UnityEngine;
using UnityEngine.UI;  
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement;
public class storyinfo : MonoBehaviour
{

    private Queue<string> sentences;
    private Queue<Sprite> images;

    public string[] textblocks;
    public Sprite[] imageblocks;

    public TMP_Text dialogueText;

    public Image imageBox;
    public bool displaydonechecker;

    void Start()
    {
        displaydonechecker = true;
        sentences = new Queue<string>();
        images = new Queue<Sprite>();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && displaydonechecker == true)
        {
            DisplayNextSentenceImage();
        }
    }

    public void StartDialogue()
    {

        sentences.Clear();
        images.Clear();

        foreach (string s in textblocks)
        {
            sentences.Enqueue(s);
        }
        foreach (Sprite i in imageblocks)
        {
            images.Enqueue(i);
        }

        DisplayNextSentenceImage();
    }

    public void DisplayNextSentenceImage(){
        if (sentences.Count == 0 || images.Count == 0)  //make sure they are the same count, use dupes if need no change to image
        {  
                SceneManager.LoadScene("Tutorial");
                return;
        }

        string sentenceshown = sentences.Dequeue();
        imageBox.sprite = images.Dequeue();
        StartCoroutine(TypeSentence(sentenceshown));
    }

    IEnumerator TypeSentence(string sentence)
    {
        displaydonechecker = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        displaydonechecker = true;
    }

}
