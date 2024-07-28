using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    public static ConversationController instance;
    public Conversation[] Conversations;

    public List<conversationScripable> conversationScripables;
    public int conversationScripableIndex;

    public CanvasGroup CanvasGroup;

    public int indexCharacter;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartConversation(int indexConversationScripable,Action action)
    {
        this.gameObject.SetActive(true);
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
        conversationScripableIndex = indexConversationScripable;
        Appear();
        ResetData();
        conversationScripables[indexConversationScripable].StartConversation(action);
    }

    [Button("Disappear")]
    public void Disappear()
    {
        for (int j = 0; j < Conversations.Length; j++)
        {
            Conversations[j].textBox.text = string.Empty;
            Conversations[j].gameObject.SetActive(false);
        }
        CanvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            this.gameObject.SetActive(false);
        });
    }

    public void Appear()
    {
        CanvasGroup.DOFade(1, 1f);
    }
    public void ResetData()
    {
        for(int j = 0;j < Conversations.Length; j++)
        {
            Conversations[j].index = 0;
        }
    }
}
public enum ConversationType
{
    singleConver,
    connectConver,
    otherConver
};
