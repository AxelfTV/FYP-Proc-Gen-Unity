using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Cube
{
    Vector3Int position;
    float d;
    int triIndex;
    Vector3[] verts;

    public Cube(Vector3Int position, float[] vertexVals, float d)
    {
        this.position = position;
        this.d = d;
        triIndex = 0;
        if (vertexVals[0] < d) triIndex += 1;
        if (vertexVals[1] < d) triIndex += 2;
        if (vertexVals[2] < d) triIndex += 4;
        if (vertexVals[3] < d) triIndex += 8;
        if (vertexVals[4] < d) triIndex += 16;
        if (vertexVals[5] < d) triIndex += 32;
        if (vertexVals[6] < d) triIndex += 64;
        if (vertexVals[7] < d) triIndex += 128;


        verts = new Vector3[]
        {
            position + new Vector3(InterpVal(vertexVals[0],vertexVals[1]), 0,1),
            position + new Vector3(1,0,InterpVal(vertexVals[2],vertexVals[1])),
            position + new Vector3(InterpVal(vertexVals[3],vertexVals[2]),0,0),
            position + new Vector3(0,0,InterpVal(vertexVals[3],vertexVals[0])),
            position + new Vector3(InterpVal(vertexVals[4],vertexVals[5]), 1,1),
            position + new Vector3(1,1,InterpVal(vertexVals[6],vertexVals[5])),
            position + new Vector3(InterpVal(vertexVals[7],vertexVals[6]),1,0),
            position + new Vector3(0,1, InterpVal(vertexVals[7],vertexVals[4])),
            position + new Vector3(0,InterpVal(vertexVals[0],vertexVals[4]), 1),
            position + new Vector3(1,InterpVal(vertexVals[1],vertexVals[5]), 1),
            position + new Vector3(1,InterpVal(vertexVals[2],vertexVals[6]), 0),
            position + new Vector3(0,InterpVal(vertexVals[3],vertexVals[7]), 0),
        };
    }

    public int[] GetTris()
    {
        int[] tris = CaveMapManager.triTable[triIndex];
        //Array.Reverse(tris);
        return tris;
    }
    public Vector3[] GetVerts()
    {

        return verts;
        
        
    }
    float InterpVal(float n, float m)
    {
        float nd = MathF.Abs(n - d);
        float md = MathF.Abs(m - d);
        return nd / (md + nd);
    }
}
