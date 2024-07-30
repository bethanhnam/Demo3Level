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

    public void StartConversation(int indexCharacterEmo,int indexConversationScripable, Action action)
    {
        this.gameObject.SetActive(true);
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
        conversationScripableIndex = indexConversationScripable;
        Appear();
        ResetData();
        DOVirtual.DelayedCall(0.8f, () => { 
        conversationScripables[indexConversationScripable].StartConversation(indexCharacterEmo, conversationScripables[indexConversationScripable].isconnectLine, action);
        });
    }
    public void Disappear()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        for (int j = 0; j < Conversations.Length; j++)
        {
            Conversations[j].textBox.text = string.Empty;
            Conversations[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < conversationScripables.Count; j++)
        {
            conversationScripables[j].indexChat = 0;
        }
        CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.4f, () =>
            {
                if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
                {
                    Stage.Instance.canInteract = true;
                }
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
            });
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            this.gameObject.SetActive(false);
        });
    }

    public void Appear()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        CanvasGroup.DOFade(1, 1f).OnComplete(() =>
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
        });
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
