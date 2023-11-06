using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid
{
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private int[,] m_gridArray;
    private TextMesh[,] m_debugTextArray;
    private Vector3 m_originPos;

    public CustomGrid(int x_width, int x_height, float x_cellSize, Vector3 x_originPos)
    {
        m_width = x_width;
        m_height = x_height;
        m_cellSize = x_cellSize;
        m_originPos = x_originPos;

        m_gridArray = new int[m_width, m_height];
        m_debugTextArray = new TextMesh[m_width, m_height];

        //LineRenderer

        /*Vector3 fromPoint, toPoint;
        Gizmos.color = Color.white;*/

        // can also use m_gridArray.GetLength(0) & (1) to get dims of array
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                Utils.S_TextMeshObj textMeshObj =
                    new Utils.S_TextMeshObj(m_gridArray[x, y].ToString(),
                    GetWorldPosition(x, y) + new Vector3(m_cellSize, m_cellSize) * .5f);
                textMeshObj.fontSize = 20;
                m_debugTextArray[x, y] = Utils.CreateWorldText(textMeshObj);
                /*fromPoint = GetWorldPosition(x, y);
                toPoint = GetWorldPosition(x, y + 1);
                Gizmos.DrawLine(fromPoint, toPoint);
                toPoint = GetWorldPosition(x + 1, y);
                Gizmos.DrawLine(fromPoint, toPoint);*/

                /*Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);*/
            }
        }
        /*fromPoint = GetWorldPosition(0, m_height);
        toPoint = GetWorldPosition(m_width, m_height);
        Gizmos.DrawLine(fromPoint, toPoint);
        fromPoint = GetWorldPosition(m_width, 0);
        Gizmos.DrawLine(fromPoint, toPoint);*/

        /*Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);*/
    }

    public int GetWidth() { return m_width; }
    public int GetHeight() { return m_height; }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_cellSize + m_originPos;
    }

    public Vector2Int GetXY(Vector3 x_worldPos)
    {
        Vector3 pos = x_worldPos - m_originPos;
        int x = Mathf.FloorToInt(pos.x / m_cellSize);
        int y = Mathf.FloorToInt(pos.y / m_cellSize);
        return new Vector2Int(x, y);
    }

    public void SetValue(int x, int y, int x_value)
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            m_gridArray[x, y] = x_value;
            m_debugTextArray[x, y].text = x_value.ToString();
        }
    }
    public void SetValue(Vector3 x_worldPos, int x_value)
    {
        Vector2Int xy = GetXY(x_worldPos);
        SetValue(xy.x, xy.y, x_value);
    }

    public int GetValue(int x, int y)
    {
        int retValue = 0;
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            retValue = m_gridArray[x, y];
        }
        return retValue;
    }
    public int GetValue(Vector3 x_worldPos)
    {
        Vector2Int xy = GetXY(x_worldPos);
        return GetValue(xy.x, xy.y);
    }
}
