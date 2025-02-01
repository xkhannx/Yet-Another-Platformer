using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMenuActions : MonoBehaviour
{
    [SerializeField] AllLevelsSO allLevels;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonListParent;

    [SerializeField] TMP_InputField newLevelNameInputField;
    [SerializeField] TMP_InputField renameLevelInputField;
    [SerializeField] GameObject levelContextMenu;

    [SerializeField] int currentLevelIndex;

    SaveLoad saver;
    private void Start()
    {
        saver = FindObjectOfType<SaveLoad>();
        UpdateLevelsList();
    }

    public void OpenLevelContextMenu(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        levelContextMenu.gameObject.SetActive(true);
        levelContextMenu.GetComponentInChildren<TMP_Text>().text = allLevels.levels[levelIndex].name;
    }

    public void CreateNewLevel(bool rename)
    {
        if (!rename)
        {
            saver.CreateEmptyLevel(newLevelNameInputField.text);
            newLevelNameInputField.text = "";
        } else
        {
            saver.RenameLevel(currentLevelIndex, renameLevelInputField.text);
            renameLevelInputField.text = "";
        }
        UpdateLevelsList();
    }

    public void DeleteLevel()
    {
        saver.DeleteLevel(currentLevelIndex);
        UpdateLevelsList();
    }

    public void OpenLevel()
    {
        FindObjectOfType<CurrentLevelInfo>().levelIndex = currentLevelIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelEditor");
    }

    private void UpdateLevelsList()
    {
        foreach (Transform child in buttonListParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < allLevels.levels.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = allLevels.levels[i].name;
            newButton.GetComponent<LevelButton>().levelButtonIndex = i;
        }
    }
}
