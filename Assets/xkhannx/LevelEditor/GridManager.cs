using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    AvailableCellPrefabs cellStash;
    LevelEditor levelEditor;
    public int gridSizeX = 200;
    public int gridSizeY = 112;
    public Cell[,,] cells;

    public GameObject[,] emptyGrid;
    Color emptyColor;

    public void InitGrid()
    {
        cellStash = FindObjectOfType<AvailableCellPrefabs>();
        levelEditor = FindObjectOfType<LevelEditor>();

        emptyColor = cellStash.emptyCellPrefab.GetComponent<SpriteRenderer>().color;
        CreateGrid();
        ConstructEmptyLevel();
    }

    private void ConstructEmptyLevel()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                emptyGrid[x, y] = Instantiate(cellStash.emptyCellPrefab, cells[x, y, 0].worldPos, Quaternion.identity, cellStash.emptyTilesParent);
            }
        }
    }

    private void CreateGrid()
    {
        emptyGrid = new GameObject[gridSizeX, gridSizeY];
        cells = new Cell[gridSizeX, gridSizeY, 5];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    Vector2 worldPos = new Vector2(x + 0.5f, y + 0.5f);
                    cells[x, y, z] = new Cell(x, y, z, worldPos, CellType.Empty);
                }
            }
        }
    }

    public List<Cell> ExtractCellList()
    {
        List<Cell> cellList = new List<Cell>();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (cells[x, y, z].cellType != CellType.Empty)
                    {
                        cellList.Add(cells[x, y, z]);
                    }

                }
            }
        }

        return cellList;
    }

    public void ReconstructLevel(List<Cell> savedCells)
    {
        for (int i = 0; i < savedCells.Count; i++)
        {
            cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z] = CopyCell(savedCells[i]);

            switch (savedCells[i].cellType)
            {
                case CellType.BlackWall:
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.backgroundWallBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.Wall:
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.wallCellBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.ColoredWall:
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.coloredBlockBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.Spike:
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.spikeCellBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.Hooks:
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.hookCellBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.PlayerStart:
                    levelEditor.playerStartCell = cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z];
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.playerStartBrush, savedCells[i].worldPos + new Vector2(1, 1), Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.Win:
                    levelEditor.winCell = cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z];
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.winTileBrush, savedCells[i].worldPos + new Vector2(1, 1), Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    break;
                case CellType.LevelBound:
                    levelEditor.boundary = cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z];
                    cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO =
                        Instantiate(cellStash.levelBoundBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.layerParents[savedCells[i].Z]);
                    SetBoundary(new Vector2Int(levelEditor.boundary.X, levelEditor.boundary.Y));
                    break;
            }

            if (cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO != null)
            {
                cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO.GetComponent<SpriteRenderer>().sortingOrder = cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].Z * 10;
                cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].cellGO.GetComponent<SpriteRenderer>().color = cells[savedCells[i].X, savedCells[i].Y, savedCells[i].Z].color;
            }
        }
    }

    Cell CopyCell(Cell orig)
    {
        return new Cell(orig.X, orig.Y, orig.Z, orig.worldPos, orig.cellType, orig.color);
    }

    public void SetBoundary(Vector2Int boundary)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                emptyGrid[x, y].GetComponent<SpriteRenderer>().color = emptyColor;
            }
        }

        Color col = Color.white;
        col.a = 0;
        for (int x = 0; x < boundary.x; x++)
        {
            emptyGrid[x, boundary.y].GetComponent<SpriteRenderer>().color = col;
        }
        for (int y = 0; y < boundary.y; y++)
        {
            emptyGrid[boundary.x, y].GetComponent<SpriteRenderer>().color = col;
        }
    }
}
