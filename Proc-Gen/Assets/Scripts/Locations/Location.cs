using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Location
{
    protected Vector2Int centre;

    public List<Vector2Int> vertices;
    public List<float> vertexValues;
    public List<Color> vertexColours;
    public List<Vector2Int> chunks;
    public List<GameObject> objects;
    protected void ConstructLocation()
    {
        CreateShape();
        SetValues();
        SetColours();
        chunks = ChunkFuncs.CheckChunks(vertices);
    }
    protected abstract void CreateShape();
    protected abstract void SetValues();
    protected abstract void SetColours();
}
