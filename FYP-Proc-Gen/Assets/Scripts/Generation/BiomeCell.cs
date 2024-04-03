using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCell
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

    public BiomeCell(Vector2Int position, Biome biome)
    {
        this.octaves = biome.octaves;
        this.lacunarity = biome.lacunarity;
        this.persistance = biome.persistance;
        this.scale = biome.scale;
        this.mult = biome.mult;
        this.heightOffset = biome.heightOffset;
        this.gradient = biome.gradient;
        this.foliage = biome.foliage;
        this.foliageMult = biome.foliageMult;
        x = position.x;
        z = position.y;
    }
}
