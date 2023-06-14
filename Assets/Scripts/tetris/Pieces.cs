using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TetrisPieces
{
    I,
    O,
    T,
    L,
    S,
}

[System.Serializable]
public struct PiecesData
{
    public TetrisPieces PiecieType;
    public Tile Tiles;
    public Vector2Int[] Cells;
}

