using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class SquaresRenderer : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    List<Vector3> vertices;
    List<int> triangles;
    Dictionary<Vector3Int, Square> squares;
    Dictionary<Vector3Int, float> vertHeights;

    NoiseMap noiseMap;
    NoiseMap3D noiseMap3D;

    public List<Biome> biomes;
    Seed seed;

    void Start()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;
        
        squares = new Dictionary<Vector3Int, Square>();
        vertHeights = new Dictionary<Vector3Int, float>();
        triangles = new List<int>();
        vertices = new List<Vector3>();

        seed = new Seed();
        noiseMap = new NoiseMap(seed, biomes);
        noiseMap3D = new NoiseMap3D(seed);

        CreateHeights();
        CreateShape();
        UpdateMesh();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mesh = new Mesh();
            col = GetComponent<MeshCollider>();
            col.sharedMesh = null;
            GetComponent<MeshFilter>().mesh = mesh;

            squares = new Dictionary<Vector3Int, Square>();
            vertHeights = new Dictionary<Vector3Int, float>();
            triangles = new List<int>();
            vertices = new List<Vector3>();

            seed = new Seed();
            noiseMap = new NoiseMap(seed, biomes);
            noiseMap3D = new NoiseMap3D(seed);

            CreateHeights();
            CreateShape();
            UpdateMesh();
        }
    }
    void CreateHeights()
    {
        int xSize = 10;
        int zSize = 10;

        
        
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                SquareCheck(x,z);
            }
        }
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {   
                /*
                squares.Add(new Vector3Int(x, 0, z), new Square(new Vector3Int(x, 0, z),
                    vertHeights[new Vector3Int(x, 0, z)],
                    vertHeights[new Vector3Int(x+1, 0, z)],
                    vertHeights[new Vector3Int(x, 0, z+1)],
                    vertHeights[new Vector3Int(x+1, 0, z+1)]
                    ));
                squares.Add(new Vector3Int(x, 1, z), new Square(new Vector3Int(x, 1, z),
                    vertHeights[new Vector3Int(x, 1, z)],
                    vertHeights[new Vector3Int(x + 1, 1, z)],
                    vertHeights[new Vector3Int(x, 1, z + 1)],
                    vertHeights[new Vector3Int(x + 1, 1, z + 1)]
                    ));
                squares.Add(new Vector3Int(x, 2, z), new Square(new Vector3Int(x, 2, z),
                    vertHeights[new Vector3Int(x, 2, z)],
                    vertHeights[new Vector3Int(x + 1, 2, z)],
                    vertHeights[new Vector3Int(x, 2, z + 1)],
                    vertHeights[new Vector3Int(x + 1, 2, z + 1)]
                    ));
                squares.Add(new Vector3Int(x, 3, z), new Square(new Vector3Int(x, 3, z),
                    vertHeights[new Vector3Int(x, 3, z)],
                    vertHeights[new Vector3Int(x + 1, 3, z)],
                    vertHeights[new Vector3Int(x, 3, z + 1)],
                    vertHeights[new Vector3Int(x + 1, 3, z + 1)]
                    ));
                */
                float h;
                int y = 0;
                while (vertHeights.TryGetValue(new Vector3Int(x,y,z), out h))
                {
                    float v1 = h;
                    float v2 = (vertHeights.TryGetValue(new Vector3Int(x + 1, y, z), out h)) ? h : v1;
                    float v3 = (vertHeights.TryGetValue(new Vector3Int(x, y, z + 1), out h)) ? h : v1;
                    float v4 = (vertHeights.TryGetValue(new Vector3Int(x + 1, y, z + 1), out h)) ? h : v1;

                    squares.Add(new Vector3Int(x, y, z), new Square(new Vector3Int(x, y, z), v1, v2, v3, v4));
                    y++;
                }



            }
        }
    }
    void SquareCheck(int x, int z)
    {
        float prevY = noiseMap3D.GetNoise(x,0,z);
        float currentY = 0;
        int y = 0;
        if (prevY <= 0)
        {
            vertHeights.Add(new Vector3Int(x, 0, z), 0);
            y++;
        }
        for(int i = 1; i < 10; i++)
        {
            currentY = noiseMap3D.GetNoise(x, i, z);
            if (currentY * prevY < 0)
            {
                vertHeights.Add(new Vector3Int(x, y, z), i - 0.5f);
                y++;
            }
        }

    }  
    void CreateShape()
    {              
        foreach (var square in squares.Values) 
        {
            int triCount = vertices.Count;
            foreach(var i in square.GetTris(triCount))
            {
                triangles.Add(i);
            }
            foreach(var v in square.GetVerts(transform.position))
            {
                vertices.Add(v);
            }
        }              
    }
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        //mesh.colors = colours;

        mesh.RecalculateNormals();
        col.sharedMesh = null;
        col.sharedMesh = mesh;

    }
}
