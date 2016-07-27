using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathGenerator : MonoBehaviour {

    public int n;//grid size
    private int powerOf = 0;//Represents the power of 2 that n is (0,1,2,3,4,5,6...)
    public int xOffset, yOffset;
    public int maxRooms = 0;
    public int roomDist = 5;
    [Range(0,20)]
    public int roomSize = 17;
    public bool addDeadEnds = false;
    [Range(0,20)]
    public int deadEndsNumb = 1;

    public int seed;
    public bool useRandomSeed = false;

    List<Vector3> path = new List<Vector3>();

    public RoomGenerator roomGenerator;

    public void Start()
    {
        GeneratePath();
    }

    void OnDrawGizmos()
    {
        if (path.Count != 0)
        {
            HilbertCurve.DrawRooms(HilbertCurve.hilbertPoints, Color.black);
            HilbertCurve.DrawRooms(path, Color.white);
            HilbertCurve.DrawPath(path, Color.red);
        }
        
    }

    public void GeneratePath()
    {
        n = lastPowerOf2(n);

        System.Random pseudoRandom = new System.Random(seed);

        xOffset = pseudoRandom.Next(0, n + 1);
        yOffset = pseudoRandom.Next(0, n + 1);
        //Vector3 pos = new Vector3(-n / 2 + .5f, 0, -n / 2 + .5f);
        Vector3 pos = new Vector3((1 - n) * (roomDist + roomSize), 0, (1 - n) * (roomDist + roomSize));
        HilbertCurve.GenerateCurve(powerOf + 1, pos, roomDist, roomSize);

        List<Vector3> subGrid = HilbertCurve.GetSubGrid(xOffset, yOffset, n);
        path = HilbertCurve.GeneratePath(HilbertCurve.hilbertPoints, subGrid, deadEndsNumb, maxRooms);
    }

    public void ChangeSeed()
    {
        seed = Random.Range(0, 50000);
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

