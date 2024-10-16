﻿using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public Sprite[] Emojis;
    public Image characterImg;
    public float textSpeed;
    public Animator animator;
    public ConversationController conversationController;

    public int index;
    public string indexLine;

    public bool readyForNextAction = false;
    public bool endOfConversation = false;

    public GameObject nextLine;
    public Action action1 = null;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        textBox.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (action1 != null && readyForNextAction == true)
            {
                AudioManager.instance.sfxSource.Stop();
                AudioManager.instance.sfxSource.loop = false;
                textSpeed = 0.05f;
                nextLine.gameObject.SetActive(false);
                readyForNextAction = false;
                index++;
                indexLine = string.Empty;
                action1();
            }
            if (!indexLine.IsNullOrWhitespace())
            {
                textSpeed = 0f;
            }
        }
    }
    public void StartDialogue(int index, Action action, string line)
    {
        try
        {
            indexLine = line;
            endOfConversation = false;
            this.index = index;
            StartCoroutine(TypeLine(line));
            CheckNextLine(action, line);
        }
        catch (Exception e) { }
    }

    IEnumerator TypeLine(String line)
    {
        AudioManager.instance.PlaySFX("Typing");
        AudioManager.instance.sfxSource.loop = true;
        nextLine.gameObject.SetActive(false);
        textBox.text = string.Empty;
        readyForNextAction = false;
        foreach (char c in line.ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    public void CheckNextLine(Action action, String line)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (textBox.text == line)
        {
            AudioManager.instance.sfxSource.Stop();
            AudioManager.instance.sfxSource.loop =false;
            nextLine.gameObject.SetActive(true);
            if (conversationController.canInteract == true)
            {
                readyForNextAction = true;
            }
            else
            {

            }
        }
        else
        {
            StartCoroutine(PlayConversation(action, line));
        }
    }
    private IEnumerator PlayConversation(Action action, String line)
    {
        if (action != null)
        {
            action1 = action;
        }
        if (index < line.Length)
        {
            yield return new WaitUntil(() => textBox.text == line);
            AudioManager.instance.sfxSource.Stop();
            AudioManager.instance.sfxSource.loop = false;
            nextLine.gameObject.SetActive(true);
            if (conversationController.canInteract == true)
            {
                readyForNextAction = true;
            }
            else
            {

            }
        }
    }
    public void ChangeEmo(int indexEmo)
    {
        characterImg.sprite = Emojis[indexEmo];
    }
}
