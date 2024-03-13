using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    bool faceUp;
    Vector3Int position;
    float[] heights;
    int axis;

    
    public Square(Vector3Int position, float h1, float h2, float h3, float h4, int axis)
    {
        this.position = position;
        heights = new float[4] { h1, h2, h3, h4 };
        
        
        this.axis = axis;

        if(axis == 1) faceUp = (position.x % 2 == 0) ? false : true;
        else if (axis == 2) faceUp = (position.y % 2 == 0) ? true : false;
        else faceUp = (position.z % 2 == 0) ? false : true;

    }

    public Vector3[] GetVerts(Vector3 worldPos)
    {
        if(axis == 1)
        {
            return new Vector3[]
        {
            new Vector3(heights[0], worldPos.y + position.y, worldPos.z + position.z),
            new Vector3(heights[1], worldPos.y + position.y + 1, worldPos.z + position.z),
            new Vector3(heights[2], worldPos.y + position.y, worldPos.z + position.z + 1),
            new Vector3(heights[3], worldPos.y + position.y + 1, worldPos.z + position.z + 1),
        };
        }
        if(axis == 2)
        {
            return new Vector3[]
        {
            new Vector3(worldPos.x + position.x, heights[0], worldPos.z + position.z),
            new Vector3(worldPos.x + position.x + 1, heights[1], worldPos.z + position.z),
            new Vector3(worldPos.x + position.x, heights[2], worldPos.z + position.z + 1),
            new Vector3(worldPos.x + position.x + 1, heights[3], worldPos.z + position.z + 1),
        };
        }
        else
        {
            return new Vector3[]
                    {
            new Vector3(worldPos.x + position.x, worldPos.y + position.y, heights[0]),
            new Vector3(worldPos.x + position.x + 1,  worldPos.y + position.y, heights[1]),
            new Vector3(worldPos.x + position.x, worldPos.y + position.y + 1, heights[2]),
            new Vector3(worldPos.x + position.x + 1, worldPos.y + position.y + 1, heights[3]),
                    };
        }
        
    }
    public int[] GetTris(int startIndex)
    {
        if (faceUp)
        {
            return new int[]
            {
                0+startIndex,2+startIndex,1+startIndex,2+startIndex,3+startIndex,1+startIndex
            };
        }
        else
        {
            return new int[]
            {
                0+startIndex,1+startIndex,2+startIndex,2+startIndex,1+startIndex,3+startIndex
            };
        }
    }
}
