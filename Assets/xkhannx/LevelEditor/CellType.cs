using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellType
{
    public const string Empty = "Empty";
    public const string LevelBound = "LevelBound";

    public const string BlackWall = "BlackWall";
    public const string Wall = "Wall";
    public const string ColoredWall = "ColoredWall";
    public const string Hooks = "Hooks";

    public const string Spike = "Spike";
    public const string PlayerStart = "PlayerStart";
    public const string Win = "Win";
}

[System.Serializable]
public struct BrushLayerStruct
{
    public string brush;
    public int savedLayer;
}
