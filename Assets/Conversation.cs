using System.Collections;
using System.Collections.Generic;
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

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        textBox.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Chatting()
    {

    }

    public void StartDialogue(int index)
    {
        this.index = index;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in textList[index].ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < textList.Count - 1)
        {
            index++;
            textBox.text = string.Empty;
            StartCoroutine (TypeLine());
        }
        else
        {
            conversationController.Disappear();
        }
    }

    public void CheckNextLine()
    {
        if(textBox.text == textList[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textBox.text = textList[index];
        }
    }
}
