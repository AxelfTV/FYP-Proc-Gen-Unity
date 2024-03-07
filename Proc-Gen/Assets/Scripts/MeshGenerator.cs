using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;
    
    Color[] colours;
    public Color roadColour = Color.white;
    public Gradient gradient;

    
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateShape();
        UpdateMesh();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) 
        {         
            CreateShape();
            UpdateMesh();
        }
    }
    void CreateShape()
    {
        vertices= new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++) 
            {

                //vertices[i] = new Vector3(x, GetNoise(x,z, seed.x, seed.z) * mult, z);
                vertices[i] = new Vector3(x, MapManager.noiseMap.GetNoise(x, z), z);
                i++;
            }
        }
        int vert = 0;
        int tris = 0;
        triangles = new int[6 * xSize * zSize];

        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + xSize + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + xSize + 1;
                triangles[5 + tris] = vert + xSize + 2;


                vert++;
                tris += 6;
            }
            vert++;
        }
        float minY = 0;
        float maxY = 0;
        foreach(Vector3 v in vertices)
        {
            if(v.y > maxY) maxY = v.y;
            if(v.y < minY) minY = -v.y; 
        }
        colours = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float gradientHeight = (vertices[i].y + minY)/(minY+maxY);
                colours[i] = gradient.Evaluate(gradientHeight);
                i++;
            }
        }
        int first = Random.Range(0, vertices.Length);
        int second = Random.Range(0, vertices.Length);
        int third = Random.Range(0, vertices.Length);
        DrawRoad(first, second);
        //DrawRoad(second, third);
        //DrawRoad(third, first);

    }
    
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;

        mesh.RecalculateNormals();
        col.sharedMesh = null;
        col.sharedMesh = mesh;
        
    }
    
    List<int> GetNeighbours(Vector3[] vertices, int index, int xSize)
    {
        var neighbours = new List<int>();
        if(index - 1 >= 0)
        {
            if (Mathf.Abs(vertices[index].x - vertices[index - 1].x) < 2)
            {
                neighbours.Add(index - 1);
            }

        }
        if (index + 1 < vertices.Length)
        {
            if (Mathf.Abs(vertices[index].x - vertices[index + 1].x) < 2)
            {
                neighbours.Add(index + 1);
            }

        }
        if (index - xSize >= 0)
        {
            
            neighbours.Add(index - xSize);
            

        }
        if (index + xSize < vertices.Length)
        {
        
            neighbours.Add(index + xSize);
        

        }
        return neighbours;
    }

    void DrawRoad(int startIndex, int endIndex)
    {
        var pathIndexes = PathGenerator.GeneratePath(vertices, startIndex, endIndex, xSize + 1);
        var pathVerts = new List<Vector3>();
        for (int n = 0; n < pathIndexes.Count; n++)
        {
            pathVerts.Add(vertices[pathIndexes[n]]);
        }
        for (int n = 0; n < pathIndexes.Count; n++)
        {
            int i = pathIndexes[n];
            if (pathVerts[n].y < 0)
            {
                vertices[i] = new Vector3(vertices[i].x, 0.2f, vertices[i].z);
                colours[i] = roadColour;
                foreach (int nI in GetNeighbours(vertices, i, xSize + 1))
                {
                    vertices[nI] = new Vector3(vertices[nI].x, 0.2f, vertices[nI].z);
                    colours[nI] = roadColour;
                }
            }
            else
            {
                vertices[i] = new Vector3(vertices[i].x, pathVerts[n].y, vertices[i].z);
                colours[i] = roadColour;
                foreach (int nI in GetNeighbours(vertices, i, xSize + 1))
                {
                    vertices[nI] = new Vector3(vertices[nI].x, pathVerts[n].y, vertices[nI].z);
                    colours[nI] = roadColour;
                }
            }
        }
    }
    

}
