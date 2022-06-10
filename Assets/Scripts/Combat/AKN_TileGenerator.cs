using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKN_TileGenerator : MonoBehaviour
{
    public Transform hexPrefab;

    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 2.4f;
    float hexHeight = 2.1f;

    Vector3 startPos;
    public List<GameObject> allTiles;

    public float obstacleChance = 0.25f;

    public void GenerateTiles()
    {
        CalcStartPos();
        CreateGrid();

        transform.position = new Vector2(2000, 2012.563f);
    }
    public void DestroyTiles()
    {
        foreach (var _tile in allTiles)
        {
            Destroy(_tile);
        }

        allTiles.Clear();

        transform.position = new Vector2(1000, 1000);
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
        for (int x = 0; x < gridHeight; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                Transform hex = Instantiate(hexPrefab) as Transform;
                allTiles.Add(hex.gameObject);
                Vector2 gridPos = new Vector2(y, x);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + x + "|" + y;
                AKN_Tile hexTile = hex.GetComponent<AKN_Tile>();

                AssignTiles(x, y, hexTile);
            }
        }
    }

    private void AssignTiles(int x, int y, AKN_Tile _hex)
    {
        if (x == 0)
        {
            _hex.tileType = TileType.Border;
        }
        else if (x == 1)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else if (y == 1 || y == 2 || y == 4 || y == 5 || y == 7 || y == 8)
            {
                _hex.tileType = TileType.EnemySpawn;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 2)
        {
            if (y == 0 || y == 1 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else if (y == 1 || y == 3 || y == 4 || y == 6 || y == 7)
            {
                _hex.tileType = TileType.EnemySpawn;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 3)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 4)
        {
            if (y == 0 || y == 1 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 5)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 6)
        {
            if (y == 0 || y == 1 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
        }
        else if (x == 7)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 8)
        {
            if (y == 0 || y == 1 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 9)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 10)
        {
            if (y == 0 || y == 1 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else if (y == 1 || y == 3 || y == 4 || y == 6 || y == 7)
            {
                _hex.tileType = TileType.FriendlySpawn;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 11)
        {
            if (y == 0 || y == 9)
            {
                _hex.tileType = TileType.Border;
            }
            else if (y == 1 || y == 2 || y == 4 || y == 5 || y == 7 || y == 8)
            {
                _hex.tileType = TileType.FriendlySpawn;
            }
            else
            {
                if (Random.value <= obstacleChance)
                {
                    _hex.tileType = TileType.Obstacle;
                }
            }
        }
        else if (x == 12)
        {
            _hex.tileType = TileType.Border;
            
        }
        
        _hex.InitializeTile();
    }

}