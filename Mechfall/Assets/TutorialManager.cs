using UnityEngine;
using UnityEngine.UI;  
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{

    private Queue<string> sentences;
    private Queue<Sprite> images;

    public string[] textblocks;
    public Sprite[] imageblocks;

    public TMP_Text dialogueText;

    public Image imageBox;

    public GameObject portal;



    public float taskcount;
    public float displaycount;
    public bool displaydonechecker;
    void Start()
    {
        displaydonechecker = true;
        taskcount = 0;
        displaycount = -2;
        sentences = new Queue<string>();
        images = new Queue<Sprite>();
        StartDialogue();
       
    }

    //task0 - jump
    //task1 - attack a
    //task2 - shoot s
    //task3 - dash d
    //task4 - go into portal

    //display-1 Yapping intro
    //display 0 jump quest
    //display 1 attack quest
    //display 2 shoot quest
    //display 3 dash quest
    //display 4 portal quest

    void Update()
    {
        if (taskcount == 0 && displaycount == 0 && Input.GetKeyDown(KeyCode.Space) && displaydonechecker == true)
        {
            DisplayNextSentenceImage();
            taskcount++;
        }
        else if (taskcount == 1 && displaycount == 1 && Input.GetKeyDown(KeyCode.A) && displaydonechecker == true)
        {
            DisplayNextSentenceImage();
            taskcount++;
        }
        else if (taskcount == 2 && displaycount == 2 && Input.GetKeyDown(KeyCode.S) && displaydonechecker == true)
        {
            DisplayNextSentenceImage();
            taskcount++;
        }
        else if (taskcount == 3 && displaycount == 3 && Input.GetKeyDown(KeyCode.D) && displaydonechecker == true)
        {
            DisplayNextSentenceImage();
            taskcount++;
            portal.SetActive(true);
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

        DoubleDisplay();
        
    }

    private void DoubleDisplay()
    {
        StartCoroutine(DisplayAutoX2());

    }

    private IEnumerator DisplayAutoX2()
    {
        DisplayNextSentenceImage();
        yield return new WaitForSeconds(5f);
        DisplayNextSentenceImage(); 
    }

    public void DisplayNextSentenceImage()
    {
        if (sentences.Count == 0 || images.Count == 0)  //make sure they are the same count, use dupes if need no change to image
        {
            return;
        }

        string sentenceshown = sentences.Dequeue();
        imageBox.sprite = images.Dequeue();
        StartCoroutine(TypeSentence(sentenceshown));
        displaycount++;
    }



    IEnumerator TypeSentence(string sentence)
    {
        displaydonechecker = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        displaydonechecker = true;
    }



}
