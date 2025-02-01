using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundCreator : MonoBehaviour
{
    AvailableCellPrefabs cellStash;

    List<Transform> origChildren = new List<Transform>();

    public List<SpriteRenderer> spriteLayer0 = new List<SpriteRenderer>();
    public List<SpriteRenderer> spriteLayer1 = new List<SpriteRenderer>();
    public List<SpriteRenderer> spriteLayer2 = new List<SpriteRenderer>();
    public List<SpriteRenderer> spriteLayer3 = new List<SpriteRenderer>();
    public List<SpriteRenderer> spriteLayer4 = new List<SpriteRenderer>();

    private void Start()
    {
        cellStash = FindObjectOfType<AvailableCellPrefabs>();

        foreach (Transform child in transform)
        {
            origChildren.Add(child);
        }
    }

    public Vector3 playerStartPos;
    public Vector3 winPos;
    public void CreateLevel(List<Cell> savedCells)
    {
        for (int i = 0; i < savedCells.Count; i++)
        {
            GameObject tempGO = null;
            switch (savedCells[i].cellType)
            {
                case CellType.BlackWall:
                    tempGO = Instantiate(cellStash.backgroundWallPrefab, savedCells[i].worldPos, Quaternion.identity, cellStash.backgroundWallParent);
                    break;
                case CellType.Wall:
                    tempGO = Instantiate(cellStash.whiteWallPrefab, savedCells[i].worldPos, Quaternion.identity, cellStash.wallCellParent);
                    break;
                case CellType.ColoredWall:
                    tempGO = Instantiate(cellStash.coloredBlockBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.wallCellParent);
                    break;
                case CellType.Spike:
                    tempGO = Instantiate(cellStash.spikeCellBrush, savedCells[i].worldPos, Quaternion.identity, cellStash.spikeCellParent);
                    break;
                case CellType.Hooks:
                    tempGO = Instantiate(cellStash.hookCellPrefab, savedCells[i].worldPos, Quaternion.identity, cellStash.hookCellParent);
                    break;
                case CellType.PlayerStart:
                    playerStartPos = savedCells[i].worldPos + new Vector2(1, 1);
                    tempGO = Instantiate(cellStash.playerStartPrefab, playerStartPos, Quaternion.identity, transform);
                    break;
                case CellType.Win:
                    winPos = savedCells[i].worldPos + new Vector2(1, 1);
                    tempGO = Instantiate(cellStash.winTilePrefab, winPos, Quaternion.identity, transform);
                    break;
            }

            if (tempGO != null)
            {
                tempGO.GetComponent<SpriteRenderer>().sortingOrder = savedCells[i].Z * 10;
                tempGO.GetComponent<SpriteRenderer>().color = savedCells[i].color;

                switch(savedCells[i].Z)
                {
                    case 0:
                        spriteLayer0.Add(tempGO.GetComponent<SpriteRenderer>());
                        break;
                    case 1:
                        spriteLayer1.Add(tempGO.GetComponent<SpriteRenderer>());
                        break;
                    case 2:
                        spriteLayer2.Add(tempGO.GetComponent<SpriteRenderer>());
                        break;
                    case 3:
                        spriteLayer3.Add(tempGO.GetComponent<SpriteRenderer>());
                        break;
                    case 4:
                        spriteLayer4.Add(tempGO.GetComponent<SpriteRenderer>());
                        break;
                }
            }
        }

        FindObjectOfType<ShadowCreator>().CreateShadows();

        FindObjectOfType<PlayInLevelEditor>().Replay();
    }

    public void DestroyLevel()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandchild in child)
            {
                Destroy(grandchild.gameObject);
            }

            if (!origChildren.Contains(child))
            {
                Destroy(child.gameObject);
            }
        }
        spriteLayer0.Clear();
        spriteLayer1.Clear();
        spriteLayer2.Clear();
        spriteLayer3.Clear();
        spriteLayer4.Clear();
    }

    Cell CopyCell(Cell orig)
    {
        return new Cell(orig.X, orig.Y, orig.Z, orig.worldPos, orig.cellType);
    }

}
