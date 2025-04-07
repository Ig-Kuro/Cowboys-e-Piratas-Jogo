using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : NetworkBehaviour
{
    //[SerializeField] private GameObject characterSelectDisplay = default;
    [SerializeField] private Transform characterPreviewParent = default;
    [SerializeField] private TMP_Text characterNameText = default;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private Character[] characters = default;
    [SerializeField] Button[] arrowButtons;

    public int currentCharacterIndex = 0;
    private List<GameObject> characterInstances = new List<GameObject>();

    public override void OnStartClient()
    {
        if (characterPreviewParent.childCount == 0)
        {
            foreach (var character in characters)
            {
                GameObject characterInstance =
                    Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);

                characterInstance.SetActive(false);

                characterInstances.Add(characterInstance);
            }
        }

        characterInstances[currentCharacterIndex].SetActive(true);
        characterNameText.text = characters[currentCharacterIndex].CharacterName;

        //characterSelectDisplay.SetActive(true);
    }

    private void Update()
    {
        characterPreviewParent.RotateAround(
            characterPreviewParent.position,
            characterPreviewParent.up,
            turnSpeed * Time.deltaTime);
    }

    public void ChangeArrowButtons(){
        foreach(Button b in arrowButtons){
            b.interactable = !b.interactable;
        }
    }

    public void Right()
    {
        characterInstances[currentCharacterIndex].SetActive(false);

        currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;

        characterInstances[currentCharacterIndex].SetActive(true);
        characterNameText.text = characters[currentCharacterIndex].CharacterName;
    }

    public void Left()
    {
        characterInstances[currentCharacterIndex].SetActive(false);

        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex += characterInstances.Count;
        }

        characterInstances[currentCharacterIndex].SetActive(true);
        characterNameText.text = characters[currentCharacterIndex].CharacterName;
    }
}