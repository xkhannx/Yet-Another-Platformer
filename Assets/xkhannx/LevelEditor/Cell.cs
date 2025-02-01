using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public string cellType = "Empty";
    public int X, Y;
    public int Z;
    public Vector2 worldPos;
    public Color color = Color.white;

    public GameObject cellGO;
    public Cell(int _gridXpos, int _gridYpos, int _layerNumber, Vector2 _worldPos, string _cellType)
    {
        X = _gridXpos;
        Y = _gridYpos;
        worldPos = _worldPos;
        cellType = _cellType;
        Z = _layerNumber;
    }
    public Cell(int _gridXpos, int _gridYpos, int _layerNumber, Vector2 _worldPos, string _cellType, Color _color)
    {
        X = _gridXpos;
        Y = _gridYpos;
        worldPos = _worldPos;
        cellType = _cellType;
        Z = _layerNumber;
        color = _color;
    }
}
