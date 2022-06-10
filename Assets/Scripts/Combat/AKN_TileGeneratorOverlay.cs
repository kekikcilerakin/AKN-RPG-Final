using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AKN_TileGeneratorOverlay : MonoBehaviour
{
    public Transform hexPrefab;

    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 2.4f;
    float hexHeight = 2.1f;

    Vector3 startPos;

    void Start()
    {
        CalcStartPos();
        CreateGrid();

        transform.position = new Vector2(2000, 2012.563f);
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, z, 0);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight;

        return new Vector3(x, z, 0);
    }

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Transform hex = Instantiate(hexPrefab) as Transform;

                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + y + "|" + x;

                AssignTiles(y, x, hex);
            }
        }
    }

    private void AssignTiles(int y, int x, Transform hex)
    {
        if (y == 0)
        {
            hex.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if (y == 1)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 2)
        {
            if (x == 0 || x == 1 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 3)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 4)
        {
            if (x == 0 || x == 1 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 5)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 6)
        {
            if (x == 0 || x == 1 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 7)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 8)
        {
            if (x == 0 || x == 1 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 9)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 10)
        {
            if (x == 0 || x == 1 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 11)
        {
            if (x == 0 || x == 9)
            {
                hex.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
        else if (y == 12)
        {
            hex.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

}
