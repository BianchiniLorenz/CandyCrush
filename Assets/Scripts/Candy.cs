using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int Column;
    public int Row;
    public int TargetX;
    public int TargetY;
    public int PrevColumn;
    public int PrevRow;
    GameObject m_otherCandy;
    Board m_gameBoard;
    Vector2 m_firstTouchPos;
    Vector2 m_lastTouchPos;
    Vector2 m_temporaryPos;
    [HideInInspector]public float SwipeAngle = 0;
    private bool m_isMatched = false;
    public Colors Color;

    public enum Colors { green, blue, purple, orange}



    void Start()
    {
        m_gameBoard = FindObjectOfType<Board>();
        TargetX = (int)transform.position.x;
        TargetY = (int)transform.position.y;
        Row = TargetY;
        Column = TargetX;
        PrevColumn = Column;
        PrevRow = Row;
        FindMatches();
    }

    void Update()
    {
        TargetX = Column;
        TargetY = Row;
        
        if(Mathf.Abs(TargetX - transform.position.x) > 0.1) //horizontal swipe
        {
            m_temporaryPos = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, m_temporaryPos, 0.4f);
        }
        else
        {
            m_temporaryPos = new Vector2(TargetX, transform.position.y);
            transform.position = m_temporaryPos;
            m_gameBoard.AllCandies[Column, Row] = gameObject;
        }

        if (Mathf.Abs(TargetY - transform.position.y) > 0.1) //vertical swipe
        {
            m_temporaryPos = new Vector2(transform.position.x , TargetY);
            transform.position = Vector2.Lerp(transform.position, m_temporaryPos, 0.4f);
        }
        else
        {
            m_temporaryPos = new Vector2(transform.position.x, TargetY);
            transform.position = m_temporaryPos;
            m_gameBoard.AllCandies[Column, Row] = gameObject;
        }

        
        if (m_isMatched)
        {
            StartCoroutine(DestroyCandy());
        }
    }

    void OnMouseDown()
    {
        m_firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        m_lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        SwipeAngle = Mathf.Atan2(m_lastTouchPos.y - m_firstTouchPos.y, m_lastTouchPos.x - m_firstTouchPos.x) * 180 / Mathf.PI;
        MoveCandy();
    }

    void MoveCandy()
    {
        if(SwipeAngle > -45 && SwipeAngle <= 45 && Column < m_gameBoard.Width -1) //right
        {
            Column++;
            m_otherCandy = m_gameBoard.AllCandies[Column, Row];
            m_otherCandy.TryGetComponent(out Candy candy);
            candy.Column--;
        }
        else if(SwipeAngle > 45 && SwipeAngle <= 135 && Row < m_gameBoard.Heigth -1) //up
        {
            Row++;
            m_otherCandy = m_gameBoard.AllCandies[Column, Row];
            m_otherCandy.TryGetComponent(out Candy candy);
            candy.Row--;
        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && Column > 0) //left
        {
            Column--;
            m_otherCandy = m_gameBoard.AllCandies[Column, Row];
            m_otherCandy.TryGetComponent(out Candy candy);
            candy.Column++;
        }
        else if (SwipeAngle < -45 && SwipeAngle >= -135 && Row > 0) //down
        {
            Row--;
            m_otherCandy = m_gameBoard.AllCandies[Column, Row];
            m_otherCandy.TryGetComponent(out Candy candy);
            candy.Row++;
        }
        StartCoroutine(CheckMove());
        FindMatches();
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(0.5f);
        if(m_otherCandy != null)
        {
            if (!m_isMatched && !m_otherCandy.GetComponent<Candy>().m_isMatched)
            {
                m_otherCandy.GetComponent<Candy>().Row = Row;
                m_otherCandy.GetComponent<Candy>().Column = Column;
                Row = PrevRow;
                Column = PrevColumn;
            }
            m_otherCandy = null;
        }
    }

    void FindMatches()
    {
        if(Column > 0 && Column < m_gameBoard.Width -1)
        {
            GameObject rightCandy = m_gameBoard.AllCandies[Column+1, Row];
            rightCandy.TryGetComponent(out Candy Rcandy); 
            GameObject leftCandy = m_gameBoard.AllCandies[Column-1, Row];
            leftCandy.TryGetComponent(out Candy Lcandy);
            if (Lcandy.Color == Color && Rcandy.Color == Color)
            {
                Lcandy.m_isMatched = true;
                Rcandy.m_isMatched = true;
                m_isMatched = true;
            }
        }

        if (Row > 0 && Row < m_gameBoard.Heigth - 1)
        {
            GameObject upCandy = m_gameBoard.AllCandies[Column, Row + 1];
            upCandy.TryGetComponent(out Candy Ucandy);
            GameObject downCandy = m_gameBoard.AllCandies[Column, Row - 1];
            downCandy.TryGetComponent(out Candy Lcandy);
            if (Lcandy.Color == Color && Ucandy.Color == Color)
            {
                Lcandy.m_isMatched = true;
                Ucandy.m_isMatched = true;
                m_isMatched = true;
            }
        }
    }

    IEnumerator DestroyCandy()
    {
        TryGetComponent(out SpriteRenderer sprite);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        StartCoroutine(m_gameBoard.CollapsRow());
    }


}
