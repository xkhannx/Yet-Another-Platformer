using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int levelButtonIndex;

    public void OpenContextualMenu()
    {
        FindObjectOfType<LevelMenuActions>().OpenLevelContextMenu(levelButtonIndex);
    }
}
