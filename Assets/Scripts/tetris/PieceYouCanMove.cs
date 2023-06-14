using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceYouCanMove : MonoBehaviour
{
    public GameBoard board { get; private set; }
    public Vector3Int position { get; private set; }
    public PiecesData data { get; private set; }
    public Vector3Int[] cells { get; private set; }

    public float StepDelay = 1f;
    public float LockDelay = 0.5f;
    float stepTime, lockTime;

    public void Initialize(GameBoard board, Vector3Int position, PiecesData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        stepTime = Time.time + StepDelay;
        lockTime = 0f;

        if(cells is null)
        {
            cells = new Vector3Int[data.Cells.Length];
        }

        for(int i = 0; i < data.Cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.Cells[i];
        }
    }

    void Update()
    {
        board.Clear(this);

        lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))          Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.D))     Move(Vector2Int.right);

        if (Time.time >= stepTime) Step();

        board.Set(this);
    }

    void Step()
    {
        stepTime = Time.time + StepDelay;
        Move(Vector2Int.down);
        if (lockTime >= LockDelay) Lock();
    }

    void Lock()
    {
        board.Set(this);
        board.SpawnPiece();
    }

    bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool isValid = board.PositionIsValid(this, newPosition);

        if (isValid)
        {
            position = newPosition;
            lockTime = 0f;
        }
        return isValid;
    }
}
