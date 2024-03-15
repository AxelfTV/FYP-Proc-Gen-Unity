using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;


public class CubesRenderer : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<Vector3Int, Cube> cubes;
    Dictionary<Vector3Int, float> vertVals;

    int size;
    Vector3Int chunkPos;
    // Start is called before the first frame update
    void Start()
    {
        size = CaveMapManager.size;
        chunkPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        CreateVertVals();
        CreateCubes();      
        CreateMesh();
    }

    
    
    
    void CreateVertVals()
    {
        vertVals = new Dictionary<Vector3Int, float>();

        for(int x =0; x <= size; x++)
        {
            for(int y=0; y<=size; y++)
            {
                for(int z =0; z <= size; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    float val = CaveMapManager.noiseMap.GetNoise(pos + chunkPos);
                    vertVals.Add(pos, val);
                    
                }
            }
        }

    }
    void CreateCubes()
    {
        cubes = new Dictionary<Vector3Int, Cube>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    float d = 0.5f;
                    Vector3Int pos = new Vector3Int(x, y, z);
                    float[] cubeVertVals = new float[8]
                    {
                        vertVals[new Vector3Int(0,0,1) + pos],
                        vertVals[new Vector3Int(1,0,1) + pos],
                        vertVals[new Vector3Int(1,0,0) + pos],
                        vertVals[new Vector3Int(0,0,0) + pos],
                        vertVals[new Vector3Int(0,1,1) + pos],
                        vertVals[new Vector3Int(1,1,1) + pos],
                        vertVals[new Vector3Int(1,1,0) + pos],
                        vertVals[new Vector3Int(0,1,0) + pos],
                    };
                    bool check = (cubeVertVals[0] < d);
                    bool isSurface = false;
                    for(int i = 1; i < 8; i++)
                    {
                        if(check != (cubeVertVals[i] < d))
                        {
                            isSurface = true;
                            break;
                        }
                    }
                    if (!isSurface) continue;
                    cubes.Add(pos, new Cube(pos, cubeVertVals, d));
                }
            }
        }
    }
    void CreateMesh()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;

        triangles = new List<int>();
        vertices = new List<Vector3>();

        foreach(var cube in cubes.Values)
        {
            int vertCount = vertices.Count;
            
            foreach(var i in cube.GetTris())
            {
                if(i != -1) triangles.Add(i + vertCount);              
            }           
            foreach (var v in cube.GetVerts())
            {
                vertices.Add(v);
            }
                                 
        }
        mesh.Clear();
        if (triangles.Count > 0)
        {
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            //mesh.colors = colours;

            mesh.RecalculateNormals();
            col.sharedMesh = null;
            col.sharedMesh = mesh;
        }
        vertVals.Clear();
        cubes.Clear();
        triangles.Clear();
        vertices.Clear();
    }
}
