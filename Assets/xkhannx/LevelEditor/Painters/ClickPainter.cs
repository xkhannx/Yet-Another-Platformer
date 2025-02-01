using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPainter : PainterBase
{ 
    private void OnEnable()
    {
        InitRefs();

        levelEditor.arrowButton.SetActive(false);
    }

    void Update()
    {
        MouseClick();

        if (clickedCell == null) return;

        if (clickedCell.cellType != levelEditor.currentBrush)
        {
            clickedCell.cellType = levelEditor.currentBrush;
            if (clickedCell.cellGO != null)
            {
                Destroy(clickedCell.cellGO);
                clickedCell.cellGO = null;
            }

            switch (levelEditor.currentBrush)
            {
                case CellType.PlayerStart:
                    if (levelEditor.playerStartCell != null)
                    {
                        levelEditor.playerStartCell.cellType = CellType.Empty;
                        Destroy(levelEditor.playerStartCell.cellGO);
                        levelEditor.playerStartCell.cellGO = null;
                    }
                    clickedCell.cellGO = Instantiate(cellStash.playerStartBrush, clickedCell.worldPos + new Vector2(1, 1), Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    levelEditor.playerStartCell = clickedCell;
                    break;
                case CellType.Win:
                    if (levelEditor.winCell != null)
                    {
                        levelEditor.winCell.cellType = CellType.Empty;
                        Destroy(levelEditor.winCell.cellGO);
                        levelEditor.winCell.cellGO = null;
                    }
                    clickedCell.cellGO = Instantiate(cellStash.winTileBrush, clickedCell.worldPos + new Vector2(1, 1), Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    levelEditor.winCell = clickedCell;
                    break;
                case CellType.LevelBound:
                    if (levelEditor.boundary != null)
                    {
                        levelEditor.boundary.cellType = CellType.Empty;
                        Destroy(levelEditor.boundary.cellGO);
                        levelEditor.boundary.cellGO = null;
                    }
                    clickedCell.cellGO = Instantiate(cellStash.levelBoundBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    levelEditor.boundary = clickedCell;
                    grid.SetBoundary(new Vector2Int(levelEditor.boundary.X, levelEditor.boundary.Y));
                    break;
            }

            if (clickedCell.cellGO != null)
                clickedCell.cellGO.GetComponent<SpriteRenderer>().sortingOrder = clickedCell.Z * 10;
        }
    }
}
