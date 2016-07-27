using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathGenerator : MonoBehaviour {

    public int n;//grid size
    public int xOffset, yOffset;
    public int maxPathSize = 0;
    public bool addDeadEnds = false;
    public int deadEndsNumb = 1;
    private int powerOf = 0;//Represents the power of 2 that n is (0,1,2,3,4,5,6...)
    int[,] grid;

    void OnDrawGizmos()
    {
        n = lastPowerOf2(n);
        Vector3 pos = new Vector3(-n / 2 + .5f, 0, -n / 2 + .5f);
        HilbertCurve.GenerateCurve(powerOf+1, pos);
        HilbertCurve.DrawRooms(HilbertCurve.hilbertPoints, Color.black);

        List<Vector3> subGrid = HilbertCurve.GetSubGrid(xOffset, yOffset, n);
        List<Vector3> path = HilbertCurve.GeneratePath(HilbertCurve.hilbertPoints, subGrid, deadEndsNumb, maxPathSize);
        HilbertCurve.DrawRooms(path, Color.white);
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

