using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEditor : MonoBehaviour
{
    public int levelIndex;
    public int currentLayer;
    public string currentBrush;
    public Color curBrushColor;

    [SerializeField] public AllLevelsSO allLevels;
    [SerializeField] TMPro.TMP_Text levelName;
    public GameObject arrowButton;

    // Painters
    [Header("Painters")]
    [SerializeField] DragPainter dragPainter;
    [SerializeField] ClickPainter clickPainter;
    [SerializeField] BrushLayerDict brushDict;

    [Header("Cell type refs")]
    [SerializeField] Image celltypeIcon;
    [SerializeField] TMP_Text celltypeText;
    [SerializeField] Sprite eraserSprite;

    [HideInInspector] public Cell playerStartCell;
    [HideInInspector] public Cell winCell;
    [HideInInspector] public Cell boundary;

    GridManager grid;
    public bool editModeEnabled;
    public bool playModeEnabled;
    private void Awake()
    {
        edge = 0.9f;

        currentBrush = "null";
        boundary = null;
        playerStartCell = null;
        winCell = null;
        curBrushColor = Color.white;

        grid = FindObjectOfType<GridManager>();

        grid.InitGrid();

        if (FindObjectOfType<CurrentLevelInfo>() == null)
            levelIndex = 0;
        else 
            levelIndex = FindObjectOfType<CurrentLevelInfo>().levelIndex;

        levelName.text = allLevels.levels[levelIndex].name;
        
        if (allLevels.levels[levelIndex].levelCells == null)
        {
            allLevels.levels[levelIndex].levelCells = new List<Cell>();
        }
        else
        {
            // Load Level
            grid.ReconstructLevel(allLevels.levels[levelIndex].levelCells);
        }

        FindObjectOfType<ColorSetter>().InitColorPicker();
    }

    public void SetBrush(string _brushType)
    {
        currentBrush = _brushType;
        celltypeText.text = currentBrush;

        dragPainter.gameObject.SetActive(false);
        clickPainter.gameObject.SetActive(false);
        editModeEnabled = true;

        switch (currentBrush)
        {
            case "null":
                celltypeIcon.sprite = null;
                celltypeText.text = "Select Brush";
                editModeEnabled = false;
                break;
            case CellType.Empty:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = eraserSprite;
                celltypeText.text = "Eraser";
                break;
            case CellType.ColoredWall:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = null;
                break;
            case CellType.Wall:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().wallCellBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.BlackWall:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().backgroundWallBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.Spike:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().spikeCellBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.PlayerStart:
                clickPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().playerStartBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.Win:
                clickPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().winTileBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.Hooks:
                dragPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().hookCellBrush.GetComponent<SpriteRenderer>().sprite;
                break;
            case CellType.LevelBound:
                clickPainter.gameObject.SetActive(true);
                celltypeIcon.sprite = FindObjectOfType<AvailableCellPrefabs>().levelBoundBrush.GetComponent<SpriteRenderer>().sprite;
                break;
        }

        for (int i = 0; i < brushDict.brushLayers.Count; i++)
        {
            if (brushDict.brushLayers[i].brush == currentBrush)
            {
                FindObjectOfType<SidePanelButtons>().SelectLayer(brushDict.brushLayers[i].savedLayer);
                break;
            }
        }
    }

    public void SaveLevel()
    {
        allLevels.levels[levelIndex].levelCells.Clear();
        allLevels.levels[levelIndex].levelCells = new List<Cell>(grid.ExtractCellList());
        UnityEditor.EditorUtility.SetDirty(allLevels.levels[levelIndex]);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Saved level " + allLevels.levels[levelIndex].name);
    }

    public float edge;
}
