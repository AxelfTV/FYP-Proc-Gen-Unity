using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCell : MonoBehaviour
{
    public int octaves = 10;
    public float lacunarity;
    public float persistance;
    public float scale;
    public float mult;
    public float heightOffset;
    public Gradient gradient;
    public int x;
    public int z;

    public List<GameObject> foliage;
    public float foliageMult;

    public BiomeCell(float lacunarity, float persistance, float scale, float mult, float heightOffset)
    {
        this.lacunarity = lacunarity;
        this.persistance = persistance;
        this.scale = scale;
        this.mult = mult;
        this.heightOffset = heightOffset;
        this.gradient = new Gradient();
        x = 0;
        z = 0;
    }
}
