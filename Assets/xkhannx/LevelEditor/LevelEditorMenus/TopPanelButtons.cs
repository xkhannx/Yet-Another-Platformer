using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPanelButtons : MonoBehaviour
{
    LevelEditor levelEditor;
    private void Start()
    {
        levelEditor = FindObjectOfType<LevelEditor>();
    }
    public void _SetEditMode(bool editEnabled)
    {
        levelEditor.editModeEnabled = editEnabled && levelEditor.currentBrush != "null";
    }

    public void _ResetBrush()
    {
        levelEditor.editModeEnabled = false;
        levelEditor.SetBrush("null");
    }

    // BRUSHES
    public void _SetBrush_Wall()
    {
        levelEditor.SetBrush(CellType.Wall);
    }
    public void _SetBrush_ColoredWall()
    {
        levelEditor.SetBrush(CellType.ColoredWall);
    }
    public void _SetBrush_BlackWall()
    {
        levelEditor.SetBrush(CellType.BlackWall);
    }
    public void _SetBrush_Spike()
    {
        levelEditor.SetBrush(CellType.Spike);
    }
    public void _SetBrush_Erase()
    {
        levelEditor.SetBrush(CellType.Empty);
    }
    public void _SetBrush_PlayerStart()
    {
        levelEditor.SetBrush(CellType.PlayerStart);
    }
    public void _SetBrush_Win()
    {
        levelEditor.SetBrush(CellType.Win);
    }
    public void _SetBrush_Hooks()
    {
        levelEditor.SetBrush(CellType.Hooks);
    }
    public void _SetBrush_Bound()
    {
        levelEditor.SetBrush(CellType.LevelBound);
    }

    // SAVE

    public void _Play()
    {
        if (levelEditor.playerStartCell == null)
        {
            Debug.Log("Add Player start tile!");
            return;
        }
        if (levelEditor.winCell == null)
        {
            Debug.Log("Add Win tile!");
            return;
        }

        FindObjectOfType<PlayInLevelEditor>().Play();
    }

    public void _ExitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
