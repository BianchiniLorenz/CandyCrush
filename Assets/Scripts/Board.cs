using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Range(1, 9)] public int Width;
    [Range(1, 15)] public int Heigth;
    public GameObject TilePrefab;
    public GameObject[] Candies; //list of every type of candies
    Vector2 m_boardPos = new Vector2(0, 0);
    Vector3 m_cam = new Vector3(0, 0, -10);
    Tiles[,] m_board;
    public GameObject[,] AllCandies; //list of every candy on the board

    void Start()
    {
        m_board = new Tiles[Width, Heigth];
        AllCandies = new GameObject[Width, Heigth];
        m_boardPos.x += (Width / 2);
        m_boardPos.y += (Heigth / 2);
        m_cam.x += m_boardPos.x;
        m_cam.y += m_boardPos.y;
        Camera.main.transform.position += m_cam;
        SetUp();
    }

    /// <summary>
    /// Create the game board
    /// </summary>
    void SetUp()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                //tiles spawn
                Vector2 TilePos = new Vector2(x, y);
                GameObject tile = Instantiate(TilePrefab, TilePos, Quaternion.identity);
                tile.transform.parent = this.transform;
                tile.name = "( " + x + " , " + y + " )";
                //candies spawn
                int index = Random.Range(0, Candies.Length);
                GameObject candy = Instantiate(Candies[index], TilePos, Quaternion.identity);
                //candy.transform.parent = this.transform;
                AllCandies[x, y] = candy;
            }
        }
    }


    public IEnumerator CollapsRow()
    {
        int nullCount = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                if (AllCandies[x, y] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    AllCandies[x, y].GetComponent<Candy>().Row -= nullCount;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
    }

}
    

    
