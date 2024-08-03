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

    public void StartConversation(int indexCharacterEmo,int indexConversationScripable,String name, Action action)
    {
        this.gameObject.SetActive(true);
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
        conversationScripableIndex = indexConversationScripable;
        Appear();
        ResetData();
        DOVirtual.DelayedCall(0.8f, () => {
            DOVirtual.DelayedCall(0.3f, () => {
                if (Stage.Instance != null)
                {
                    Stage.Instance.canInteract = false;
                }
            });
            conversationScripables[indexConversationScripable].StartConversation(indexCharacterEmo, conversationScripables[indexConversationScripable].isconnectLine, action);
        });
    }
    public void Disappear()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        for (int j = 0; j < Conversations.Length; j++)
        {
            Conversations[j].textBox.text = string.Empty;
            Conversations[j].nextLine.gameObject.SetActive(false);
            Conversations[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < conversationScripables.Count; j++)
        {
            conversationScripables[j].indexChat = 0;
        }
        CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.25f, () =>
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
    public void DeteleSlideCoversation()
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        for (int j = 0; j < conversationScripables[conversationScripableIndex].listCharacters.Count; j++)
        {
            Destroy(conversationScripables[conversationScripableIndex].listCharacters[j]);
            if(j ==  conversationScripableIndex - 1)
            {
                conversationScripables[conversationScripableIndex].listCharacters.Clear();
            }
        }
        for (int j = 0; j < conversationScripables.Count; j++)
        {
            conversationScripables[j].indexChat = 0;
        }
        CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.25f, () =>
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
