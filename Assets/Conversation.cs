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
    public Image characterImg;
    public List<string> textList;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (action1 != null)
                {
                    readyForNextAction = false;
                    index++;
                    action1();
                }
                //if (conversationController.conversationScripables[conversationController.conversationScripableIndex].conversationType == ConversationType.singleConver)
                //{
                //    readyForNextAction = false;
                //    NextLine();
                //}
            }
        }   
    }
    public void StartDialogue(int index,Action action)
    {
        endOfConversation = false;
        this.index = index;
        StartCoroutine(TypeLine());
        CheckNextLine(action);
    }

    IEnumerator TypeLine()
    {
        textBox.text = string.Empty;
        readyForNextAction = false;
        foreach (char c in textList[index].ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < textList.Count-1)
        {
            index++;
            textBox.text = string.Empty;
            StartCoroutine (TypeLine());
        }
        else
        {
            endOfConversation = true;
        }
    }

    public void CheckNextLine(Action action)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (textBox.text == textList[index])
        {
            readyForNextAction = true;
        }
        else
        {
            StartCoroutine(PlayConversation(action));
        }
    }
    private IEnumerator PlayConversation(Action action)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (index < textList.Count)
        {
            yield return new WaitUntil(() => textBox.text == textList[index]);
              readyForNextAction = true;
        }
    }
}
