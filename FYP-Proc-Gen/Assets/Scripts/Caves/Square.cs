using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    bool faceUp;
    Vector3Int position;
    float[] heights;

    public Square(Vector3Int position ,float height)
    {
        this.position = position;
        heights = new float[4] { height, height, height, height };
        faceUp = (position.y % 2 == 0) ? true : false;
    }
    public Square(Vector3Int position, float h1, float h2, float h3, float h4)
    {
        this.position = position;
        heights = new float[4] { h1, h2, h3, h4 };
        faceUp = (position.y % 2 == 0) ? true : false;
    }

    public Vector3[] GetVerts(Vector3 worldPos)
    {
        return new Vector3[]
        {
            new Vector3(worldPos.x + position.x, heights[0], worldPos.z + position.z),
            new Vector3(worldPos.x + position.x + 1, heights[1], worldPos.z + position.z),
            new Vector3(worldPos.x + position.x, heights[2], worldPos.z + position.z + 1),
            new Vector3(worldPos.x + position.x + 1, heights[3], worldPos.z + position.z + 1),
        };
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
