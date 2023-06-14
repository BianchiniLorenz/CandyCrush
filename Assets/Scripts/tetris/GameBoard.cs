using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    public PiecesData[] PiecesArray;
    public Tilemap BoardTileMap { get; private set; }
    public PieceYouCanMove ActivePiece { get; private set; }
    public Vector3Int SpawnPosition;
    public Vector2Int BoardSize = new Vector2Int(10, 20);
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2);
            return new RectInt(position, BoardSize);
        }
    }

    void Awake()
    {
        BoardTileMap = GetComponentInChildren<Tilemap>();
        ActivePiece = GetComponentInChildren<PieceYouCanMove>();
    }

    void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int randomIndex = Random.Range(0, PiecesArray.Length);
        PiecesData piece = PiecesArray[randomIndex];
        ActivePiece.Initialize(this, SpawnPosition, piece);
        Set(ActivePiece);
    }
       
    public void Set(PieceYouCanMove piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            BoardTileMap.SetTile(tilePosition, piece.data.Tiles);
        }
    }

    public void Clear(PieceYouCanMove piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            BoardTileMap.SetTile(tilePosition, null);
        }
    }

    public bool PositionIsValid(PieceYouCanMove piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (BoardTileMap.HasTile(tilePosition)) return false; //overlap

            if (!bounds.Contains((Vector2Int)tilePosition)) return false; //out of bounds
        }
        return true;
    }
}
