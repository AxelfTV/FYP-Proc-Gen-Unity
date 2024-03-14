using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Cube
{
    Vector3Int position;
    
    int triIndex;

    public Cube(Vector3Int position, float[] vertexVals, float d)
    {
        this.position = position;
        triIndex = 0;
        if (vertexVals[0] < d) triIndex += 1;
        if (vertexVals[1] < d) triIndex += 2;
        if (vertexVals[2] < d) triIndex += 4;
        if (vertexVals[3] < d) triIndex += 8;
        if (vertexVals[4] < d) triIndex += 16;
        if (vertexVals[5] < d) triIndex += 32;
        if (vertexVals[6] < d) triIndex += 64;
        if (vertexVals[7] < d) triIndex += 128;
    }

    public int[] GetTris()
    {
        int[] tris = CaveMapManager.triTable[triIndex];
        //Array.Reverse(tris);
        return tris;
    }
    public Vector3[] GetVerts()
    {
        
        return new Vector3[]
        {
            position + new Vector3(0.5f, 0,1),
            position + new Vector3(1,0,0.5f),
            position + new Vector3(0.5f,0,0),
            position + new Vector3(0,0,0.5f),
            position + new Vector3(0.5f, 1,1),
            position + new Vector3(1,1,0.5f),
            position + new Vector3(0.5f,1,0),
            position + new Vector3(0,1, 0.5f),
            position + new Vector3(0,0.5f, 1),
            position + new Vector3(1,0.5f, 1),
            position + new Vector3(1,0.5f, 0),
            position + new Vector3(0,0.5f, 0),
        };
        
        
    }
}
