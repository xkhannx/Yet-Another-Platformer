using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPainter : PainterBase
{
    private void OnEnable()
    {
        InitRefs();

        levelEditor.arrowButton.SetActive(false);
    }

    void Update()
    {
        MouseDrag();

        if (clickedCell == null) return;
        
        if (clickedCell.cellType != levelEditor.currentBrush)
        {
            if (clickedCell.cellType == CellType.PlayerStart)
            {
                levelEditor.playerStartCell = null;
            }
            if (clickedCell.cellType == CellType.Win)
            {
                levelEditor.winCell = null;
            }

            clickedCell.cellType = levelEditor.currentBrush;
            if (clickedCell.cellGO != null)
            {
                Destroy(clickedCell.cellGO);
                clickedCell.cellGO = null;
            }

            switch (levelEditor.currentBrush)
            {
                case CellType.ColoredWall:
                    clickedCell.cellGO = Instantiate(cellStash.coloredBlockBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    clickedCell.cellGO.GetComponent<SpriteRenderer>().color = levelEditor.curBrushColor;
                    clickedCell.color = levelEditor.curBrushColor;
                    break;
                case CellType.Wall:
                    clickedCell.cellGO = Instantiate(cellStash.wallCellBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    break;
                case CellType.BlackWall:
                    clickedCell.cellGO = Instantiate(cellStash.backgroundWallBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    break;
                case CellType.Spike:
                    clickedCell.cellGO = Instantiate(cellStash.spikeCellBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    break;
                case CellType.Hooks:
                    clickedCell.cellGO = Instantiate(cellStash.hookCellBrush, clickedCell.worldPos, Quaternion.identity, cellStash.layerParents[clickedCell.Z]);
                    break;
            }

            if (clickedCell.cellGO != null)
                clickedCell.cellGO.GetComponent<SpriteRenderer>().sortingOrder = clickedCell.Z * 10;
        }
    }
}
