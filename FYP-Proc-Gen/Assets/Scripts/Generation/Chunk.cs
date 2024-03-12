using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;


public class Chunk : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    Vector3[] vertices;
    int[] triangles;

    public int initX;
    public int initZ;
    public int chunkSeed;
    public Vector2Int index;
    Color[] colours;

    Dictionary<Vector2Int, GameObject> foliage;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;
        chunkSeed = GetChunkSeed(index);
        foliage = new Dictionary<Vector2Int, GameObject>();

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        int xSize = MapManager.xSize;
        int zSize = MapManager.zSize;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Random.InitState(MapManager.seed.value + chunkSeed);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, MapManager.noiseMap.GetNoise(x + initX, z + initZ), z);
                GameObject plant = MapManager.noiseMap.GetPlant(vertices[i] + new Vector3(initX,0,initZ));
                if (plant != null && vertices[i].y > 0)
                {
                    Vector3 position = vertices[i] + new Vector3(initX, 0, initZ);
                    GameObject obj = Instantiate(plant, position, Quaternion.Euler(new Vector3(0,Random.Range(0f,360f),0)));
                    obj.transform.parent = transform;
                    foliage.Add(new Vector2Int((int)position.x, (int)position.z), obj);
                }
                i++;              
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[6 * xSize * zSize];

        for (int z = 0; z < zSize; z++)
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
        foreach (Vector3 v in vertices)
        {
            if (v.y > maxY) maxY = v.y;
            if (v.y < minY) minY = -v.y;
        }
        colours = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                colours[i] = MapManager.noiseMap.GetColor(vertices[i] + new Vector3(initX, 0, initZ));
                i++;
            }
        }
        
        

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
    public void RenderLocation(Location location)
    {
        for(int i = 0; i < location.vertices.Count; i++)
        {
            int posIndex = ChunkFuncs.PositionToIndex(location.vertices[i], index);
            if(posIndex != -1)
            {
                GameObject plant;
                if (foliage.TryGetValue(location.vertices[i], out plant))
                {
                    foliage.Remove(location.vertices[i]);
                    Destroy(plant);
                }
                if(location.vertexValues != null)
                {
                    vertices[posIndex].y = location.vertexValues[i];
                }
                if(location.vertexColours != null)
                {
                    colours[posIndex] = location.vertexColours[i];
                }
                if(location.objects != null)
                {
                    Vector3 position = vertices[posIndex] + new Vector3(initX, 0, initZ);
                    if (location.objects[i]!=null)
                    {
                        
                        GameObject obj = Instantiate(location.objects[i], position, Quaternion.identity);
                        obj.transform.parent = transform;
                        foliage.Add(new Vector2Int((int)position.x, (int)position.z), obj);
                    }
                }
            }
        }
        UpdateMesh();
    }  
    int GetChunkSeed(Vector2Int chunkIndex)
    {
        int x = chunkIndex.x;
        int y = chunkIndex.y;
        if (x >= y) return x + y * y;
        else return x * x + x + y;        
    }
    
}
