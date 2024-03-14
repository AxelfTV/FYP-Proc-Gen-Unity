using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class SquaresRenderer : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    List<Vector3> vertices;
    List<int> triangles;
    Dictionary<Vector3Int, Square> squaresX;
    Dictionary<Vector3Int, Square> squaresY;
    Dictionary<Vector3Int, Square> squaresZ;

    Dictionary<Vector3Int, float> vertHeightsX;
    Dictionary<Vector3Int, float> vertHeightsY;
    Dictionary<Vector3Int, float> vertHeightsZ;

    NoiseMap noiseMap;
    NoiseMap3D noiseMap3D;

    public List<Biome> biomes;
    Seed seed;

    public int size = 20;
    float noiseScale = 0.9f;
    void Start()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;
        
        squaresX = new Dictionary<Vector3Int, Square>();
        squaresY = new Dictionary<Vector3Int, Square>();
        squaresZ = new Dictionary<Vector3Int, Square>();

        vertHeightsX = new Dictionary<Vector3Int, float>();
        vertHeightsY = new Dictionary<Vector3Int, float>();
        vertHeightsZ = new Dictionary<Vector3Int, float>();

        triangles = new List<int>();
        vertices = new List<Vector3>();

        seed = CaveMapManager.seed;
        noiseMap = new NoiseMap(seed, biomes);
        noiseMap3D = new NoiseMap3D(seed, Vector3.zero);

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

            squaresX = new Dictionary<Vector3Int, Square>();
            squaresY = new Dictionary<Vector3Int, Square>();
            squaresZ = new Dictionary<Vector3Int, Square>();

            vertHeightsX = new Dictionary<Vector3Int, float>();
            vertHeightsY = new Dictionary<Vector3Int, float>();
            vertHeightsZ = new Dictionary<Vector3Int, float>();
            triangles = new List<int>();
            vertices = new List<Vector3>();

            seed = CaveMapManager.seed;
            noiseMap = new NoiseMap(seed, biomes);
            noiseMap3D = new NoiseMap3D(seed, Vector3.zero);

            CreateHeights();
            CreateShape();
            UpdateMesh();
        }
    }
    void CreateHeights()
    {
        int xSize = size;
        int zSize = size;

        float angle = 2;
        
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                SquareCheckX(x, z);
                SquareCheckY(x, z);
                SquareCheckZ(x, z);
            }
        }
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {   
                
                float h;
                int y = 0;
                if(vertHeightsY.TryGetValue(new Vector3Int(x, y, z), out h))
                {
                    int curY = y;                   
                    float v1 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x + 1, curY, z), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x, curY, z + 1), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x + 1, curY, z + 1), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresY.Add(new Vector3Int(x, curY, z), new Square(new Vector3Int(x, curY, z), v1, v2, v3, v4, 2));
                }
                y++;
                while (vertHeightsY.TryGetValue(new Vector3Int(x,y,z), out h))
                {
                    int curY = y;
                    y++;
                    float v1 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x + 1, curY, z), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x, curY, z + 1), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsY.TryGetValue(new Vector3Int(x + 1, curY, z + 1), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresY.Add(new Vector3Int(x, curY, z), new Square(new Vector3Int(x, curY, z), v1, v2, v3, v4,2));
                }

                y = 0;
                if(vertHeightsX.TryGetValue(new Vector3Int(y, x, z), out h))
                {
                    int curY = y;
                    
                    float v1 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x + 1, z), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x, z + 1), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x + 1, z + 1), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresX.Add(new Vector3Int(curY, x, z), new Square(new Vector3Int(curY, x, z), v1, v2, v3, v4, 1));
                }
                y++;
                while (vertHeightsX.TryGetValue(new Vector3Int(y, x, z), out h))
                {
                    int curY = y;
                    y++;
                    float v1 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x+1, z), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x, z + 1), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsX.TryGetValue(new Vector3Int(curY, x + 1, z + 1), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresX.Add(new Vector3Int(curY, x, z), new Square(new Vector3Int(curY, x, z), v1, v2, v3, v4,1));
                }
                y = 0;
                if(vertHeightsZ.TryGetValue(new Vector3Int(x, z, y), out h))
                {
                    int curY = y;
                    y++;
                    float v1 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x + 1, z, curY), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x, z + 1, curY), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x + 1, z + 1, curY), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresZ.Add(new Vector3Int(x, z, curY), new Square(new Vector3Int(x, z, curY), v1, v2, v3, v4, 3));
                }
                y++;
                while (vertHeightsZ.TryGetValue(new Vector3Int(x, z, y), out h))
                {
                    int curY = y;
                    y++;
                    float v1 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x + 1, z, curY), out h)) continue;
                    float v2 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x, z + 1, curY), out h)) continue;
                    float v3 = h;
                    if (!vertHeightsZ.TryGetValue(new Vector3Int(x + 1, z + 1, curY), out h)) continue;
                    float v4 = h;
                    if (Mathf.Abs(v2 - v1) > angle || Mathf.Abs(v3 - v1) > angle || Mathf.Abs(v4 - v1) > angle) continue;
                    squaresZ.Add(new Vector3Int(x, z, curY), new Square(new Vector3Int(x, z, curY), v1, v2, v3, v4, 3));
                }
            }
        }
    }
    
    void SquareCheckX(int y, int z)
    {
        float prevX = noiseMap3D.GetNoise((0 + transform.position.x) * noiseScale, (y + transform.position.y) * noiseScale, (z + transform.position.z) * noiseScale);
        float currentX;
        int x = 0;
        if (prevX <= 0)
        {
            //vertHeightsX.Add(new Vector3Int(0, y, z), 0);
            x++;
        }
        for(int i = 1; i < size; i++)
        {
            currentX = noiseMap3D.GetNoise((i+transform.position.x) * noiseScale, (y+transform.position.y) * noiseScale, (z+ transform.position.z) * noiseScale);
            if (currentX * prevX < 0)
            {
                vertHeightsX.Add(new Vector3Int(x, y, z), i - 0.5f);
                x++;
            }
            prevX = currentX;
        }

    }
    void SquareCheckY(int x, int z)
    {
        float prevY = noiseMap3D.GetNoise((x + transform.position.x) * noiseScale, (0 + transform.position.y) * noiseScale, (z + transform.position.z) * noiseScale);
        float currentY;
        int y = 0;
        if (prevY <= 0)
        {
            //vertHeightsY.Add(new Vector3Int(x, 0, z), 0);
            y++;
        }
        for (int i = 1; i < size; i++)
        {
            currentY = noiseMap3D.GetNoise((x + transform.position.x) * noiseScale, (i + transform.position.y) * noiseScale, (z + transform.position.z) * noiseScale);
            if (currentY * prevY < 0)
            {
                vertHeightsY.Add(new Vector3Int(x, y, z), i - 0.5f);
                y++;
            }
            prevY = currentY;
        }

    }
    void SquareCheckZ(int x, int y)
    {
        float prevZ = noiseMap3D.GetNoise((x + transform.position.x) * noiseScale, (y + transform.position.y) * noiseScale, (0 + transform.position.z) * noiseScale);
        float currentZ;
        int z = 0;
        if (prevZ <= 0)
        {
            //vertHeightsZ.Add(new Vector3Int(x, y, 0), 0);
            z++;
        }
        for (int i = 1; i < size; i++)
        {
            currentZ = noiseMap3D.GetNoise((x + transform.position.x) * noiseScale, (y + transform.position.y) * noiseScale, (i + transform.position.z) * noiseScale);
            if (currentZ * prevZ < 0)
            {
                vertHeightsZ.Add(new Vector3Int(x, y, z), i - 0.5f);
                z++;
            }
            prevZ = currentZ;
        }

    }
    void CreateShape()
    {              
        foreach (var square in squaresY.Values) 
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
        foreach (var square in squaresX.Values)
        {
            int triCount = vertices.Count;
            foreach (var i in square.GetTris(triCount))
            {
                triangles.Add(i);
            }
            foreach (var v in square.GetVerts(transform.position))
            {
                vertices.Add(v);
            }
        }
        foreach (var square in squaresZ.Values)
        {
            int triCount = vertices.Count;
            foreach (var i in square.GetTris(triCount))
            {
                triangles.Add(i);
            }
            foreach (var v in square.GetVerts(transform.position))
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
