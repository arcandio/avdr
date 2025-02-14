using TMPro;
using UnityEngine;

/// <summary>
/// Button for a charactger in the character list
/// </summary>
public class UiCharacterButton : UiNavigation
{
    public TextMeshProUGUI buttonText;

    [SerializeField] private int index;
    [SerializeField] private string characterName;

    [SerializeField] private CharacterManager characterManager;

    public void SetupCharacterButton(CharacterManager cmTemp, int indexTemp, string nameTemp) {
        characterManager = cmTemp;
        index = indexTemp;
        characterName = nameTemp;
        buttonText.text = characterName;
    }

    public void ClickedCharacterButton() {
        characterManager.SelectCharacter(index);
        NavigateToPage(PageName.CharacterEditPage);
    }
}