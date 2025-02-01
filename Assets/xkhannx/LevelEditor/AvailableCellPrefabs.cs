using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCellPrefabs : MonoBehaviour
{
    [Header("Brushes - Wall")]
    public GameObject emptyCellPrefab;
    public GameObject wallCellBrush;
    public GameObject backgroundWallBrush;
    public GameObject coloredBlockBrush;

    [Header("Brushes - Mechanics")]
    public GameObject spikeCellBrush;
    public GameObject hookCellBrush;

    [Header("Brushes - Singles")]
    public GameObject playerStartBrush;
    public GameObject winTileBrush;
    public GameObject levelBoundBrush;
    
    [Header("========================================")]
    public GameObject whiteWallPrefab;
    public GameObject backgroundWallPrefab;
    public GameObject hookCellPrefab;
    public GameObject playerStartPrefab;
    public GameObject winTilePrefab;

    [Header("Parents")]
    public Transform emptyTilesParent;
    public Transform backgroundWallParent;
    public Transform wallCellParent;
    public Transform spikeCellParent;
    public Transform hookCellParent;

    [Header("Layers")]
    public List<Transform> layerParents;
}
