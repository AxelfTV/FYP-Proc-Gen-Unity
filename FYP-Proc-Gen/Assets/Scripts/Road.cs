using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Location
{    
    public Vector2Int end;

    Color colour;
    public Road(Vector2Int start, Vector2Int end, Color colour)
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
        vertexValues = null;       
    }
    protected override void SetColours()
    {
        vertexColours = new List<Color>();
        foreach(var vertex in vertices)
        {
            vertexColours.Add(colour);
        }
    }
}
