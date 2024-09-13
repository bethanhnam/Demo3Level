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

    public bool canInteract = true;

    public List<conversationScripable> conversationScripables;
    public int conversationScripableIndex;

    public CanvasGroup CanvasGroup;

    public int indexCharacter;

    public List<Conversation> listCharacters = new List<Conversation>();


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

    public void StartConversation(int indexCharacterEmo, int indexConversationScripable, String name, Action action,bool setpos = false)
    {
        if (UIManagerNew.Instance.GamePlayPanel != null)
        {
            UIManagerNew.Instance.GamePlayPanel.DeactiveTime();
        }
        if (Stage.Instance != null)
        {
            Stage.Instance.canInteract = false;
        }
        this.gameObject.SetActive(true);
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
        conversationScripableIndex = indexConversationScripable;
        Appear();
        ResetData();
        DOVirtual.DelayedCall(0.8f, () =>
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                if (Stage.Instance != null)
                {
                    Stage.Instance.canInteract = false;
                }
            });
            conversationScripables[indexConversationScripable].StartConversation(indexCharacterEmo, conversationScripables[indexConversationScripable].isconnectLine, action, setpos);
        });
    }
    public void Disappear()
    {
        if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
        {
            UIManagerNew.Instance.GamePlayPanel.ActiveTime();
        }
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
        if (listCharacters.Count > 0)
        {
            for (int j = listCharacters.Count -1; j >=0; j--)
            {
                var x = listCharacters[j]; 
                listCharacters.RemoveAt(j);
                x.gameObject.SetActive(false);
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
        for (int j = 0; j < Conversations.Length; j++)
        {

            Conversations[j].index = 0;
        }
    }

    public void SetInteractable(bool status)
    {
        GameManagerNew.Instance.conversationController.canInteract = status;
    }
    //public void StartConversationNoBackGround(int alphaBackground,int indexCharacterEmo, int indexConversationScripable, String name, Action action, bool setpos = false)
    //{
    //    if (UIManagerNew.Instance.GamePlayPanel != null)
    //    {
    //        UIManagerNew.Instance.GamePlayPanel.DeactiveTime();
    //    }
    //    if (Stage.Instance != null)
    //    {
    //        Stage.Instance.canInteract = false;
    //    }
    //    this.gameObject.SetActive(true);
    //    CanvasGroup.interactable = true;
    //    CanvasGroup.blocksRaycasts = true;
    //    conversationScripableIndex = indexConversationScripable;
    //    Appear();
    //    ResetData();
    //    DOVirtual.DelayedCall(0.8f, () =>
    //    {
    //        DOVirtual.DelayedCall(0.3f, () =>
    //        {
    //            if (Stage.Instance != null)
    //            {
    //                Stage.Instance.canInteract = false;
    //            }
    //        });
    //        conversationScripables[indexConversationScripable].StartConversation(indexCharacterEmo, conversationScripables[indexConversationScripable].isconnectLine, action, setpos);
    //    });
    //}



}
public enum ConversationType
{
    singleConver,
    connectConver,
    otherConver
};
