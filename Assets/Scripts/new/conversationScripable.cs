using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "conversationScripable", order = 1)]
public class conversationScripable : ScriptableObject
{
    public ConversationType conversationType;
    public int numOfChat;

    public int indexChar;
    public int indexChat;

    public void StartConversation(Action action)
    {
        if (conversationType == ConversationType.singleConver)
        {
            CreateLineForSingle(0, action);
        }
        if (conversationType == ConversationType.connectConver)
        {
            CreateLineForConnect(ChangeCharacter(indexChar), action);
        }
    }

    public void CreateLineForSingle(int i, Action action)
    {
        if (indexChat < numOfChat)
        {
            if (!ConversationController.instance.Conversations[i].gameObject.activeSelf)
            {
                Appear(i);
            }
            DOVirtual.DelayedCall(0.3f, () =>
            {
                ConversationController.instance.Conversations[i].StartDialogue(ConversationController.instance.Conversations[i].index, () =>
                {
                    indexChat++;
                    CreateLineForSingle(indexChar, action);
                });
            });
        }
        else
        {
            ConversationController.instance.Disappear();
            action();
        }
    }
    public void CreateLineForConnect(int i, Action action)
    {
        if (indexChat < numOfChat)
        {
            if (!ConversationController.instance.Conversations[i].gameObject.activeSelf)
            {
                Appear(i);
            }
            DOVirtual.DelayedCall(0.3f, () =>
            {
                ConversationController.instance.Conversations[i].StartDialogue(ConversationController.instance.Conversations[i].index, () =>
                {
                    indexChat++;
                    CreateLineForConnect(ChangeCharacter(indexChar), action);
                });
            });
        }
        else
        {
            ConversationController.instance.Disappear();
            action();
        }
    }
    public int ChangeCharacter(int currentCharacter)
    {
        var x = 0;
        if (currentCharacter == 0)
        {
            indexChar = 1;
            x = 1;
        }
        else if (currentCharacter == 1)
        {
            indexChar = 0;
            x = 0;
        }
        return x;
    }
    public void Appear(int indexCharacter)
    {
        ConversationController.instance.Conversations[indexCharacter].gameObject.SetActive(true);
        ConversationController.instance.Conversations[indexCharacter].animator.enabled = true;
    }
}
