using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorSetter : MonoBehaviour
{
    LevelEditor levelEditor;
    Color curColor;
    [SerializeField] TMP_InputField hexInput;
    public void InitColorPicker()
    {
        levelEditor = FindObjectOfType<LevelEditor>();

        curColor = levelEditor.allLevels.levels[levelEditor.levelIndex].camColor;
        Camera.main.backgroundColor = curColor;

        hexInput.text = ColorUtility.ToHtmlStringRGB(curColor);
    }

    public void UpdateColor()
    {
        if (FindObjectOfType<Hotkeys>().blockColor) return;

        Color newCol;
        if (ColorUtility.TryParseHtmlString("#" + hexInput.text, out newCol))
            Camera.main.backgroundColor = newCol;
    }

    public void SaveColor()
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString("#" + hexInput.text, out newCol))
        {
            curColor = newCol;
            if (FindObjectOfType<Hotkeys>().blockColor)
            {
                levelEditor.curBrushColor = newCol;
                FindObjectOfType<Hotkeys>().blockColor = false;
            }
            else
            {
                levelEditor.allLevels.levels[levelEditor.levelIndex].camColor = newCol;
            }
        }
    }

    public void DiscardColor()
    {
        if (FindObjectOfType<Hotkeys>().blockColor) return;

        Camera.main.backgroundColor = curColor;
        hexInput.text = ColorUtility.ToHtmlStringRGB(curColor);
    }
}
