using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class HilbertCurve {

    private static int size;
    public static List<Vector3> hilbertPoints = new List<Vector3>();
    private static int roomDist, roomSize;

    public enum DIRECTION
    {
        UP,
        LEFT,
        DOWN,
        RIGHT,
    };

    public static List<Vector3> GetSubGrid(int xOffSet, int yOffset, int gridSize)
    {
        Vector3 startPos = hilbertPoints[0] + Vector3.right * (roomDist +roomSize)* xOffSet + Vector3.forward * (roomDist + roomSize) * yOffset;
        List<Vector3> subGrid = new List<Vector3>();
        int finalIndex = (int)Mathf.Pow(gridSize, 2);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                subGrid.Add(startPos + Vector3.right *(roomDist+roomSize)* y);
            }
            startPos += (Vector3.forward * (roomDist + roomSize));
        }

        return subGrid;
    }

    public static List<Vector3> GeneratePath(List<Vector3> grid, List<Vector3> subGrid, int maxRooms)
    {
        if (maxRooms == 0)
            maxRooms = subGrid.Count;

        Vector3 lastPos = Vector3.zero;
        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i < grid.Count; i++)
        {
            if (subGrid.Contains(grid[i]))
            {
                if (lastPos == Vector3.zero)
                {
                    lastPos = grid[i];
                    path.Add(lastPos);
                }
                else if (grid[i] == lastPos + Vector3.right*(roomDist + roomSize) || grid[i] == lastPos + Vector3.left * (roomDist + roomSize) || grid[i] == lastPos + Vector3.forward * (roomDist + roomSize) || grid[i] == lastPos + Vector3.back * (roomDist + roomSize))
                {
                    path.Add(grid[i]);
                    lastPos = grid[i];
                    if (path.Count >= maxRooms)
                        return path;
                }
            }
        }

        return path;
    }

    public static List<Vector3> GeneratePath(List<Vector3> grid, List<Vector3> subGrid, int maxDeadEnds, int maxPathSize)
    {
        return AddDeadEnds(GeneratePath(grid, subGrid, maxPathSize), subGrid, maxDeadEnds);
    }

    private static List<Vector3> AddDeadEnds(List<Vector3> path, List<Vector3> subGrid, int maxDeadEnds)
    {
        List<Vector3> neighboors = new List<Vector3>();
        List<Vector3> newPath = new List<Vector3>(path);
        if (maxDeadEnds == 0)
            return newPath;

        int changeIndex = 1;
        int addedDeadEnds = 0;

        for (int i = 0; i < path.Count-1; i++)
        {
            neighboors.Clear();
            neighboors.Add(path[i] + Vector3.right*(roomDist + roomSize));
            neighboors.Add(path[i] + Vector3.left * (roomDist + roomSize));
            neighboors.Add(path[i] + Vector3.forward * (roomDist + roomSize));
            neighboors.Add(path[i] + Vector3.back * (roomDist + roomSize));

            for (int j = 0; j < neighboors.Count; j++)
            {
                if (!newPath.Contains(neighboors[j]) && subGrid.Contains(neighboors[j]))
                {
                    newPath.Insert(i + changeIndex, neighboors[j]);
                    newPath.Insert(i + 1 + changeIndex, path[i]);
                    changeIndex+=2;
                    addedDeadEnds++;
                    if (addedDeadEnds == maxDeadEnds)
                        return newPath;
                    break;
                }
            }
        }

        return newPath;
    }

    public static void DrawPath(List<Vector3> path, Color color)
    {
        Gizmos.color = color;
        for (int i = 0; i < path.Count-1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }

    public static void DrawRooms(List<Vector3> rooms, Color color)
    {
        Gizmos.color = color;
        for (int i = 0; i < rooms.Count; i++)
        {
            Gizmos.DrawCube(rooms[i], Vector3.one * roomSize);
        }
    }

    public static void GenerateCurve(int level, Vector3 pos, int _roomDist, int _roomSize, DIRECTION direction = DIRECTION.UP)
    {
        size = (int)Mathf.Pow(2, level);

        hilbertPoints.Clear();

        hilbertPoints.Add(pos);

        roomDist = _roomDist;
        roomSize = _roomSize;

        Vector3 newPos = pos;

        HilbertAlgorithm(level, ref newPos, direction);
    }

    public static void HilbertAlgorithm(int level, ref Vector3 pos, DIRECTION direction)
    {
        if (level == 0)
            return;

        if (level == 1)
        {
            switch (direction)
            {
                case DIRECTION.LEFT:
                    ConnectLine(DIRECTION.RIGHT, ref pos);      
                    ConnectLine(DIRECTION.DOWN, ref pos);      
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    break;
                case DIRECTION.RIGHT:
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    ConnectLine(DIRECTION.UP, ref pos);
                    ConnectLine(DIRECTION.RIGHT, ref pos);
                    break;
                case DIRECTION.UP:
                    ConnectLine(DIRECTION.DOWN, ref pos);
                    ConnectLine(DIRECTION.RIGHT, ref pos);
                    ConnectLine(DIRECTION.UP, ref pos);
                    break;
                case DIRECTION.DOWN:
                    ConnectLine(DIRECTION.UP, ref pos);
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    ConnectLine(DIRECTION.DOWN, ref pos);
                    break;
            } /* switch */
        }
        else
        {
            switch (direction)
            {
                case DIRECTION.LEFT:
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.UP);
                    ConnectLine(DIRECTION.RIGHT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.LEFT);
                    ConnectLine(DIRECTION.DOWN, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.LEFT);
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.DOWN);
                    break;
                case DIRECTION.RIGHT:
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.DOWN);
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.RIGHT);
                    ConnectLine(DIRECTION.UP, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.RIGHT);
                    ConnectLine(DIRECTION.RIGHT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.UP);
                    break;
                case DIRECTION.UP:
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.LEFT);
                    ConnectLine(DIRECTION.DOWN, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.UP);
                    ConnectLine(DIRECTION.RIGHT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.UP);
                    ConnectLine(DIRECTION.UP, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.RIGHT);
                    break;
                case DIRECTION.DOWN:
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.RIGHT);
                    ConnectLine(DIRECTION.UP, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.DOWN);
                    ConnectLine(DIRECTION.LEFT, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.DOWN);
                    ConnectLine(DIRECTION.DOWN, ref pos);
                    HilbertAlgorithm(level - 1, ref pos, DIRECTION.LEFT);
                    break;
            } /* switch */
        } /* if */
    }

    static void ConnectLine(DIRECTION direction, ref Vector3 pos)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                pos += (Vector3.back*(roomDist + roomSize));
                hilbertPoints.Add(pos);
                break;
            case DIRECTION.RIGHT:
                pos += (Vector3.right * (roomDist + roomSize));
                hilbertPoints.Add(pos);
                break;
            case DIRECTION.LEFT:
                pos += (Vector3.left * (roomDist + roomSize));
                hilbertPoints.Add(pos);
                break;
            case DIRECTION.DOWN:
                pos += (Vector3.forward * (roomDist + roomSize));
                hilbertPoints.Add(pos);
                break;
        }
    }
}
