using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotkeys : MonoBehaviour
{
    LevelEditor levelEditor;
    [SerializeField] Button playButton;
    [SerializeField] Button eraseButton;
    [SerializeField] GameObject editModeCanvas;
    [SerializeField] GameObject playModeCanvas;
    [SerializeField] GameObject colorPicker;
    
    public bool blockColor = false;
    public bool hidden = false;
    private void Start()
    {
        levelEditor = FindObjectOfType<LevelEditor>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!levelEditor.playModeEnabled)
            {
                playButton.onClick.Invoke();
            }
            else
            {
                FindObjectOfType<PlayInLevelEditor>().EnterEditMode();
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hidden = !hidden;
            if (levelEditor.playModeEnabled)
            {
                playModeCanvas.SetActive(!hidden);
            }
            else
            {
                editModeCanvas.SetActive(!hidden);
            }
            
            levelEditor.edge = hidden ? 1 : 0.9f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!levelEditor.playModeEnabled)
            {
                eraseButton.onClick.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!levelEditor.playModeEnabled)
            {
                levelEditor.SaveLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!levelEditor.playModeEnabled)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!levelEditor.playModeEnabled)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!levelEditor.playModeEnabled)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!levelEditor.playModeEnabled)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!levelEditor.playModeEnabled)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(4);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!levelEditor.playModeEnabled)
            {
                blockColor = true;
                colorPicker.SetActive(true);
                levelEditor.editModeEnabled = false;
            }
        }
    }
}
