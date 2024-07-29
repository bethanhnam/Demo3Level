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
    public String[] character1Lines;

    public ConversationType conversationType;

    public int indexChar;
    public int indexChat;

    public void StartConversation(int indexCharacterEmo,Action action)
    {
        if (conversationType == ConversationType.singleConver)
        {
            CreateLineForSingle(indexCharacterEmo,indexChar, action);
        }
        if (conversationType == ConversationType.connectConver)
        {
            CreateLineForConnect(ChangeCharacter(indexChar), indexCharacterEmo, action);
        }
    }

    public void CreateLineForSingle(int indexCharacterEmo, int i, Action action)
    {
        if (indexChat < character1Lines.Length)
        {
            if (!ConversationController.instance.Conversations[i].gameObject.activeSelf)
            {
                Appear(indexCharacterEmo,i);
            }
            DOVirtual.DelayedCall(0.3f, () =>
            {
                ConversationController.instance.Conversations[i].StartDialogue(ConversationController.instance.Conversations[i].index, () =>
                {
                    indexChat++;
                    CreateLineForSingle(indexCharacterEmo, indexChar, action);
                }, character1Lines[indexChat]);
            });
        }
        else
        {
            ConversationController.instance.Disappear();
            action();
        }
    }
    public void CreateLineForConnect(int i,int indexCharacterEmo, Action action)
    {
        if (indexChat < character1Lines.Length)
        {
            if (!ConversationController.instance.Conversations[i].gameObject.activeSelf)
            {
                Appear(indexCharacterEmo, i);
            }
            DOVirtual.DelayedCall(0.3f, () =>
            {
                ConversationController.instance.Conversations[i].StartDialogue(ConversationController.instance.Conversations[i].index, () =>
                {
                    indexChat++;
                    CreateLineForConnect(ChangeCharacter(indexChar), indexCharacterEmo, action);
                }, character1Lines[indexChat]);
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
    public void Appear(int indexCharacterEmo, int indexCharacter)
    {
        ConversationController.instance.Conversations[indexCharacter].ChangeEmo(indexCharacterEmo);
        ConversationController.instance.Conversations[indexCharacter].gameObject.SetActive(true);
        ConversationController.instance.Conversations[indexCharacter].animator.enabled = true;
    }
}
