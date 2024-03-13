using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverWater : MonoBehaviour
{
    Mesh mesh;
    MeshCollider col;
    Vector3[] vertices;
    int[] triangles;
   
    Color[] colours;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();

        transform.Translate(Vector3.down * (transform.position.y) + Vector3.left * 7.5f + Vector3.back * 7.5f);
    }

    void CreateShape()
    {
        int xSize = 15;
        int zSize = 15;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, MapManager.noiseMap.GetNoise(x + transform.position.x - 7.5f, z + transform.position.z - 7.5f) -0.5f, z);                                
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
                colours[i] = Color.blue;
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
        //col.sharedMesh = null;
        //col.sharedMesh = mesh;

    }
}
