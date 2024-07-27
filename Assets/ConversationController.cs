using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    public List<Conversation> conversations;
    public CanvasGroup CanvasGroup;

    public int indexCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Appear(int indexCharacter)
    {
        conversations[indexCharacter].gameObject.SetActive(true);
        CanvasGroup.DOFade(1, 0.5f);
        conversations[indexCharacter].animator.enabled = true;
    }
    
    public void SetChatting(int indexCharacter,int line)
    {
        conversations[indexCharacter].StartDialogue(line);
    }

    public void Disappear()
    {
        CanvasGroup.DOFade(0, 1);
        for (int i = 0; i < conversations.Count; i++)
        {
            conversations[i].gameObject.SetActive(false);
        }
    }
}
