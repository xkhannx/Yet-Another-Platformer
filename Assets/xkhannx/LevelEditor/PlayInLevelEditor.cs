using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInLevelEditor : MonoBehaviour
{
    LevelEditor levelEditor;
    PlaygroundCreator creator;
    GridManager grid;
    Hotkeys hotkeys;

    [SerializeField] GameObject editModeCanvas;
    [SerializeField] GameObject playModeCanvas;
    [SerializeField] GameObject playerPrefab;

    GameObject player;
    private void Start()
    {
        levelEditor = GetComponent<LevelEditor>();
        creator = FindObjectOfType<PlaygroundCreator>();
        grid = FindObjectOfType<GridManager>();
        hotkeys = FindObjectOfType<Hotkeys>();
    }
    public void Play()
    {
        levelEditor.editModeEnabled = false;
        levelEditor.playModeEnabled = true;

        editModeCanvas.SetActive(false);
        playModeCanvas.SetActive(!hotkeys.hidden);

        grid.gameObject.SetActive(false);
        creator.CreateLevel(grid.ExtractCellList());
    }

    public void Replay()
    {
        Vector3 playerStartPos = creator.playerStartPos + new Vector3(0, -0.5f, 0);

        player = Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
        
        FindObjectOfType<GrayDoor>().OpenDoor();
    }

    public void EnterEditMode()
    {
        player.GetComponent<PlayerController>().Die();
        player = null;

        levelEditor.editModeEnabled = levelEditor.currentBrush != "null";
        levelEditor.playModeEnabled = false;

        editModeCanvas.SetActive(!hotkeys.hidden);
        playModeCanvas.SetActive(false);

        creator.DestroyLevel();
        grid.gameObject.SetActive(true);
    }
}
