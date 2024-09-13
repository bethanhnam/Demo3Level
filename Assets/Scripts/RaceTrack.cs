
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class RaceTrack : MonoBehaviour
{
    public CharacterRace[] characterRace;

    public Transform target;

    public CharacterRace maincharacter;

    float distance;
    float step;

    List<int> movedIndices = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        distance = Vector2.Distance(target.position, this.transform.position);
        step = distance / 10;
    }

    // Update is called once per frame
    void Update()
    {


    }
    [Button("Move Characters")]
    public void moveRandomCharacter()
    {
        int numCharactersToMove = Random.Range(1, characterRace.Length + 1);

        for (int i = 0; i < numCharactersToMove; i++)
        {
            int randIndex;
            do
            {
                randIndex = Random.Range(0, characterRace.Length);
            } while (movedIndices.Contains(randIndex));

            // Add the selected index to the moved list
            movedIndices.Add(randIndex);

            move(characterRace[randIndex]);
            characterRace[randIndex].topIndex += 1;
            characterRace[randIndex].changeText(characterRace[randIndex].topIndex);
        }
        movedIndices.Clear();
        SavePos();
    }

    public void move(CharacterRace characterRace)
    {
        characterRace.transform.position = new Vector3(characterRace.transform.position.x, characterRace.transform.position.y + step, 1);
        //characterRace.transform.DOMoveY(characterRace.transform.position.y + step, 0.5f);
    }
    [Button("ResetCharacterPos")]
    public void ResetCharacterPos()
    {
        for (int i = 0; i < characterRace.Length; i++)
        {
            characterRace[i].transform.position = new Vector3(characterRace[i].transform.position.x, this.transform.position.y, 1);
            characterRace[i].topIndex = 0;
            characterRace[i].topText.text = 0.ToString();
        }
        maincharacter.transform.position = new Vector3(maincharacter.transform.position.x, this.transform.position.y, 1);
        maincharacter.topIndex = 0;
        maincharacter.topText.text = maincharacter.topIndex.ToString();
        SavePos();
    }
    [Button("SavePos")]
    public void SavePos()
    {
        for (int i = 0; i < characterRace.Length; i++)
        {
            // Save position string to PlayerPrefs with the character's name as the key
            PlayerPrefs.SetInt("characterRace " + i, characterRace[i].topIndex);

            // Optionally reset the character's position or perform other actions
            characterRace[i].transform.position = new Vector3(characterRace[i].transform.position.x, characterRace[i].transform.position.y, 1);
        }

        // Save position string to PlayerPrefs with the character's name as the key
        PlayerPrefs.SetInt("maincharacter", maincharacter.topIndex);

        // Optionally reset the character's position or perform other actions
        maincharacter.transform.position = new Vector3(maincharacter.transform.position.x, maincharacter.transform.position.y, 1);

    }
    [Button("LoadPos")]
    public void LoadPos()
    {
        for (int i = 0; i < characterRace.Length; i++)
        {
            if (PlayerPrefs.HasKey("characterRace " + i))
            {
                int topString = PlayerPrefs.GetInt("characterRace " + i);

                // Set the character's position

                characterRace[i].topIndex = topString;
                characterRace[i].changeText(topString);   

                Vector3 position = new Vector3(characterRace[i].transform.position.x,this.transform.position.y + (step * characterRace[i].topIndex),1);
                characterRace[i].transform.position = position;
            }
        }
        int topString1 = PlayerPrefs.GetInt("maincharacter");
        // Set the character's position
        maincharacter.topIndex = topString1;
        maincharacter.changeText(topString1);
        
        Vector3 position1 = new Vector3(maincharacter.transform.position.x, this.transform.position.y + (step * maincharacter.topIndex), 1);
        maincharacter.transform.position = position1;
    }
    [Button("moveMainCharacter")]
    public void moveMainCharacter()
    {
        move(maincharacter);
        maincharacter.topIndex += 1;
        maincharacter.changeText(maincharacter.topIndex);
        SavePos();
    }
}
