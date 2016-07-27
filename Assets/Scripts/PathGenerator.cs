using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathGenerator : MonoBehaviour {

    public int n;//grid size
    private int powerOf = 0;//Represents the power of 2 that n is (0,1,2,3,4,5,6...)
    public int xOffset, yOffset;
    public int maxRooms = 0;
    public int roomDist = 5;
    public bool addDeadEnds = false;
    public int deadEndsNumb = 1;

    public RoomGenerator roomGenerator;

    void OnDrawGizmos()
    {
        int roomSize = roomGenerator.width;

        n = lastPowerOf2(n);
        Vector3 pos = new Vector3(-n / 2 + .5f, 0, -n / 2 + .5f);
        HilbertCurve.GenerateCurve(powerOf+1, pos, roomDist, roomSize);
        HilbertCurve.DrawRooms(HilbertCurve.hilbertPoints, roomSize, Color.black);

        List<Vector3> subGrid = HilbertCurve.GetSubGrid(xOffset, yOffset, n, roomDist, roomSize);
        List<Vector3> path = HilbertCurve.GeneratePath(HilbertCurve.hilbertPoints, subGrid, roomDist, roomSize, deadEndsNumb, maxRooms);
        HilbertCurve.DrawRooms(path, roomSize, Color.white);
        HilbertCurve.DrawPath(path, Color.red);
    }

    public void Draw()
    {
        xOffset = Random.Range(0, n + 1);
        yOffset = Random.Range(0, n + 1);
    }

    int lastPowerOf2(int n)
    {
        int result = 1;
        powerOf = 0;

        while ((n>>=1) != 0)
        {
            result <<= 1;
            powerOf++;
        }

        return result;
    }

    int nextPowerOf2(int n)
    {
        int result = 2;
        powerOf = 0;

        while ((n >>= 1) != 0)
        {
            result <<= 1;
            powerOf++;
        }

        return result;
    }

}

