using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidePanelButtons : MonoBehaviour
{
    LevelEditor levelEditor;
    AvailableCellPrefabs cellStash;

    [SerializeField] List<Image> layerButtons = new List<Image>();
    [SerializeField] List<Image> visButtons = new List<Image>();

    [SerializeField] BrushLayerDict brushDict;
    void Start()
    {
        levelEditor = FindObjectOfType<LevelEditor>();
        layerButtons[levelEditor.currentLayer].color = Color.green;

        cellStash = FindObjectOfType<AvailableCellPrefabs>();
    }

    public void SelectLayer(int newLayer)
    {
        layerButtons[levelEditor.currentLayer].color = Color.white;
        levelEditor.currentLayer = newLayer;
        layerButtons[newLayer].color = Color.green;

        if (levelEditor.currentBrush == "null")
        {
            return;
        }

        BrushLayerStruct updatedBrushLayer = new BrushLayerStruct();
        updatedBrushLayer.brush = levelEditor.currentBrush;
        updatedBrushLayer.savedLayer = newLayer;

        for (int i = 0; i < brushDict.brushLayers.Count; i++)
        {
            if (brushDict.brushLayers[i].brush == levelEditor.currentBrush)
            {
                brushDict.brushLayers[i] = updatedBrushLayer;
                break;
            }
        }

        UnityEditor.EditorUtility.SetDirty(brushDict);
    }

    public void SetVisible(int layerNumber)
    {
        bool visible = cellStash.layerParents[layerNumber].gameObject.activeInHierarchy;

        cellStash.layerParents[layerNumber].gameObject.SetActive(!visible);
        visButtons[layerNumber].color = visible ? Color.red : Color.white;
    }
}
