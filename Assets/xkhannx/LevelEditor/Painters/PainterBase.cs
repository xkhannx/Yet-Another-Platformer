using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterBase : MonoBehaviour
{
    Camera cam;
    protected GridManager grid;
    protected LevelEditor levelEditor;
    protected Cell clickedCell;
    protected AvailableCellPrefabs cellStash;

    protected void InitRefs()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (grid == null)
        {
            grid = FindObjectOfType<GridManager>();
        }

        if (levelEditor == null)
        {
            levelEditor = FindObjectOfType<LevelEditor>();
        }

        if (cellStash == null)
        {
            cellStash = FindObjectOfType<AvailableCellPrefabs>();
        }
    }

    protected void MouseDrag()
    {
        if (!levelEditor.editModeEnabled) return;

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePixelPos = Input.mousePosition;
            if (mousePixelPos.y > Screen.height * levelEditor.edge) return;
            if (mousePixelPos.y < 0) return;
            if (mousePixelPos.x > Screen.width * levelEditor.edge) return;
            if (mousePixelPos.x < 0) return;

            clickedCell = CellFromMouse(mousePixelPos);
        }
        else
        {
            clickedCell = null;
        }
    }

    protected void MouseClick()
    {
        if (!levelEditor.editModeEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePixelPos = Input.mousePosition;
            if (mousePixelPos.y > Screen.height * levelEditor.edge) return;
            if (mousePixelPos.y < 0) return;
            if (mousePixelPos.x > Screen.width * levelEditor.edge) return;
            if (mousePixelPos.x < 0) return;

            clickedCell = CellFromMouse(mousePixelPos);
        }
        else
        {
            clickedCell = null;
        }
    }

    Cell CellFromMouse(Vector2 mousePixelPos)
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(mousePixelPos);

        int x = Mathf.FloorToInt(mousePos.x);
        int y = Mathf.FloorToInt(mousePos.y);
        return grid.cells[x, y, levelEditor.currentLayer];
    }
}
