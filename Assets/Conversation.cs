using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public Sprite[] Emojis;
    public Image characterImg;
    public float textSpeed;
    public Animator animator;
    public ConversationController conversationController;

    public int index;
    public bool readyForNextAction = false;
    public bool endOfConversation = false;
    public Action action1 = null;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        textBox.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForNextAction)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (action1 != null)
                {
                    readyForNextAction = false;
                    index++;
                    action1();
                }
            }
        }   
    }
    public void StartDialogue(int index,Action action,string line)
    {
        endOfConversation = false;
        this.index = index;
        StartCoroutine(TypeLine(line));
        CheckNextLine(action,line);
    }

    IEnumerator TypeLine(String line)
    {
        textBox.text = string.Empty;
        readyForNextAction = false;
        foreach (char c in line.ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    //public void NextLine()
    //{
    //    if(index < textList.Count-1)
    //    {
    //        index++;
    //        textBox.text = string.Empty;
    //        StartCoroutine (TypeLine());
    //    }
    //    else
    //    {
    //        endOfConversation = true;
    //    }
    //}

    public void CheckNextLine(Action action,String line)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (textBox.text == line)
        {
            readyForNextAction = true;
        }
        else
        {
            StartCoroutine(PlayConversation(action, line));
        }
    }
    private IEnumerator PlayConversation(Action action, String line)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (index < line.Length)
        {
            yield return new WaitUntil(() => textBox.text == line);
              readyForNextAction = true;
        }
    }
    public void ChangeEmo(int indexEmo)
    {
        characterImg.sprite = Emojis[indexEmo];
    }
}
