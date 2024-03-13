using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Location
{
    public Vector2Int end;
    GameObject water;
    
    public River(Vector2Int start, Vector2Int end, GameObject water)
    {
        centre = start;
        this.end = end;
        this.water = water;
        ConstructLocation();
    }
    protected override void CreateShape()
    {
        objects = new List<GameObject>();
        vertices = NoisePathGenerator.GeneratePath(centre, end);
        for(int i = 0; i< vertices.Count; i++)
        {
            if(i % 10 == 0) objects.Add(water);
            else if (i == vertices.Count - 1) objects.Add(water);
            else objects.Add(null);

        }
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
