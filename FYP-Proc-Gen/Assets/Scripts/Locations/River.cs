using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Location
{
    public Vector2Int end;

    
    public River(Vector2Int start, Vector2Int end)
    {
        centre = start;
        this.end = end;
        ConstructLocation();
    }
    protected override void CreateShape()
    {
        vertices = NoisePathGenerator.GeneratePath(centre, end);
    }
    protected override void SetValues()
    {
        vertexValues = new List<float>();
        foreach (var vertex in vertices)
        {
            vertexValues.Add(MapManager.noiseMap.GetNoise(vertex.x, vertex.y) - 5);
        }
    }
    protected override void SetColours()
    {
        vertexColours = null;
    }
}
