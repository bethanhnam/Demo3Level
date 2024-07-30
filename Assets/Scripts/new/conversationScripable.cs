using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "conversationScripable", order = 1)]
public class conversationScripable : ScriptableObject
{
    public String[] character1Lines;

    public ConversationType conversationType;

    private int indexChar;
    public int indexChat;
    public bool isconnectLine;

    public MyCharacter selectedCharacter;
    public enum MyCharacter
    {
        Mom,
        Child,
        Mom2
    }

    public int ChangeToInt()
    {
        int x =0;
        switch (selectedCharacter)
        {
            case MyCharacter.Child:
                x= 0;
                return 0;
            case MyCharacter.Mom:
                x = 1;
                return 1;
            case MyCharacter.Mom2:
                x = 2;
                return 2;
        }
        return x;
    }

    public void StartConversation(int indexCharacterEmo, bool isconnectLine, Action action)
    {
        if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
        {
            Stage.Instance.canInteract = false;
        }
        if (conversationType == ConversationType.singleConver)
        {
            CreateLineForSingle(indexCharacterEmo, ChangeToInt(), isconnectLine, action);
        }
        if (conversationType == ConversationType.connectConver)
        {
            CreateLineForConnect(ChangeCharacter(ChangeToInt()), indexCharacterEmo, isconnectLine, action);
        }
    }

    public void CreateLineForSingle(int indexCharacterEmo, int i,bool isconnectLine, Action action)
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
                    CreateLineForSingle(indexCharacterEmo, ChangeToInt(), isconnectLine, action);
                }, character1Lines[indexChat]);
            });
        }
        else
        {
            if (!isconnectLine)
            {
                ConversationController.instance.Disappear();
                action();
            }
            else
            {
                action();
            }
        }
    }
    public void CreateLineForConnect(int i,int indexCharacterEmo,bool isconnectLine, Action action)
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
                    CreateLineForConnect(ChangeCharacter(indexChar), indexCharacterEmo, isconnectLine, action);
                }, character1Lines[indexChat]);
            });
        }
        else
        {
            if (!isconnectLine)
            {
                ConversationController.instance.Disappear();
                action();
                
            }
            else
            {
                action();
               
            }
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
