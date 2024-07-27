using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    public List<Conversation> conversations;
    public CanvasGroup CanvasGroup;
    public ConversationType ConversationType;

    public int indexCharacter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("OnClickChangeLog")]
    public void OnClickChangeLog(int IndexCharacter)
    {
        if (ConversationType == ConversationType.singleConver)
        {
            this.indexCharacter = IndexCharacter;
            conversations[this.indexCharacter].CheckNextLine();
        }
        else if (ConversationType == ConversationType.connectConver)
        {
            conversations[ChangeCharacter(this.indexCharacter)].CheckNextLine();
        }
        else
        {
            // chưa tính tới 
        }

    }

    public int ChangeCharacter(int currentCharacter)
    {
        var x = 0;
        if (currentCharacter == 0)
        {
            this.indexCharacter = 1;
            x = 1;
        }
        else if (currentCharacter == 1)
        {
            this.indexCharacter = 0;
            x =  0;
        }
        return x;
    }

    [Button("Appear")]
    public void Appear(int indexCharacter)
    {
        this.indexCharacter = indexCharacter;
        conversations[indexCharacter].gameObject.SetActive(true);
        CanvasGroup.DOFade(1, 0.5f);
        conversations[indexCharacter].animator.enabled = true;
    }

    [Button("SetChatting")]
    public void SetChatting(int indexCharacter, int line)
    {
        this.indexCharacter = indexCharacter;
        conversations[indexCharacter].StartDialogue(line);
    }

    [Button("Disappear")]
    public void Disappear()
    {
        CanvasGroup.DOFade(0, 1);
        for (int i = 0; i < conversations.Count; i++)
        {
            conversations[i].textBox.text = string.Empty;
            conversations[i].gameObject.SetActive(false);
        }
    }
}
public enum ConversationType
{
    singleConver,
    connectConver,
    otherConver
};
