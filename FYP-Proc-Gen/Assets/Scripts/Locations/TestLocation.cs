using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLocation : Location
{
    GameObject obj;

    public TestLocation(Vector2Int centre, GameObject obj)
    {
        this.centre = centre;
        this.obj = obj;

        ConstructLocation();
    }
    protected override void CreateShape()
    {
        objects = new List<GameObject>();
        vertices = new List<Vector2Int>();
        for(int x = -10; x < 11; x++)
        {
            for(int z = -10; z < 11; z++)
            {
                if (!(z == 0 && x == 0) && !(Mathf.Abs(z) == 8 && Mathf.Abs(x) == 8)) objects.Add(null);
                
                else objects.Add(obj);
                vertices.Add(centre + new Vector2Int(x, z));
            }
        }
    }
    protected override void SetValues()
    {
        vertexValues = new List<float>();
        float centreHeight = MapManager.noiseMap.GetNoise(centre.x, centre.y);
        foreach(var vertex in vertices)
        {
            vertexValues.Add(centreHeight + 1);
        }
    }
    protected override void SetColours()
    {
        vertexColours = new List<Color>();
        foreach (var vertex in vertices)
        {
            vertexColours.Add(Color.red);
        }
    }
}
