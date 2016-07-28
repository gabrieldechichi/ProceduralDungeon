using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{

    public int width;
    public int height;
    public int borderSize = 2;

    public int seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;
    [Range(1,10)]
    public int smoothInterations = 2;

    int[,] map;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothInterations; i++)
        {
            SmoothMap();
        }

        ProcessMap();

        int[,] borderedMap = GenerateBorders(borderSize);

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap);
    }


    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = (int)Time.time;
        }

        System.Random pseudoRandom = new System.Random(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;//Encourage walls on the borders
                    //map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    private int[,] GenerateBorders(int borderSize)
    {
        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)//inside the map
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                else
                    borderedMap[x, y] = 1;
            }
        }

        return borderedMap;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX,neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];//store the already checked tiles
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX-1; x < tile.tileX+1; x++)
            {
                for (int y = tile.tileY-1; y < tile.tileY+1; y++)
                {
                    if (IsInMapRange(x, y) && (y==tile.tileY || x==tile.tileX))
                    {
                        if (mapFlags[x,y] == 0 && map[x,y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for(int x = 0;x<width;x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(mapFlags[x,y] == 0 && map[x,y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach(Coord tile in newRegion)
                        map[tile.tileX, tile.tileY] = 1;
                }
            }
        }

        return regions;
    }

    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);
        int wallThesholdCount = 50;//If any wall region has less than 50 tiles, it will be discarted
        foreach(List<Coord> wallRegion in wallRegions)
        {
            if(wallRegion.Count < wallThesholdCount)
            {
                foreach (Coord tile in wallRegion)
                    map[tile.tileX, tile.tileY] = 0;
            }
        }

    }

    bool IsInMapRange(int x, int y)
    {
        return x >=0 && x < width && y >= 0 && y < height;
    }

    struct Coord
    {
        public int tileX, tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }


    /*void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }*/

}