using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NoiseMap
{
    Seed seed;
    Dictionary<Vector2Int, BiomeCell> biomeDict;

    float interpDist = 40;
    public NoiseMap(Seed seed, Dictionary<Vector2Int, BiomeCell> biomes)
    {
        this.seed = seed;
        this.biomeDict = biomes;
        
        
        
    }
    
    public float GetNoise(float x, float z)
    {
        List<BiomeCell> biomes = GetCurrentBiomes(x, z);
        BiomeCell closestBiome = FindClosestBiome(x, z,biomes);
        float closestDist = GetDist(x, z, closestBiome);

        List<BiomeCell> closeBiomes = new List<BiomeCell>();
        List<float> floats = new List<float>();
        foreach (BiomeCell biome in biomes)
        {
            float distDif = Mathf.Abs(GetDist(x, z, biome) - closestDist);
            if (distDif < interpDist)
            {
                closeBiomes.Add(biome);
                floats.Add(distDif);
            }
        }
        if (closeBiomes.Count <= 1) return GetBiomeNoise(x, z, closestBiome);
        
        
        float noiseSum = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            floats[i] = (interpDist - floats[i]) / interpDist;
            
        }
        float floatsSum = floats.Sum();
        for (int i = 0; i < floats.Count; i++)
        {
            noiseSum += (floats[i]/floatsSum) * GetBiomeNoise(x, z, closeBiomes[i]);

        }
        
        return noiseSum;
        
    }
    
    
    float GetBiomeNoise(float x, float z, BiomeCell tempBiome)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        for (int i = 0; i < tempBiome.octaves; i++)
        {
            float sampleX = x / tempBiome.scale * frequency;
            float sampleZ = z / tempBiome.scale * frequency;
            float perlinValue = Mathf.PerlinNoise(sampleX + seed.x, sampleZ + seed.z) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= tempBiome.persistance;
            frequency *= tempBiome.lacunarity;
        }
        return noiseHeight * tempBiome.mult + tempBiome.heightOffset;
    }
    public Color GetColor(Vector3 vert)
    {
        List<BiomeCell> biomes = GetCurrentBiomes(vert.x, vert.z);
        BiomeCell closestBiome = FindClosestBiome(vert.x, vert.z, biomes);
        float closestDist = GetDist(vert.x, vert.z, closestBiome);

        List<BiomeCell> closeBiomes = new List<BiomeCell>();
        List<float> floats = new List<float>();
        foreach (BiomeCell biome in biomes)
        {
            float distDif = Mathf.Abs(GetDist(vert.x, vert.z, biome) - closestDist);
            if (distDif < interpDist)
            {
                closeBiomes.Add(biome);
                floats.Add(distDif);
            }
        }
        if (closeBiomes.Count <= 1) return GetBiomeColor(vert.x, vert.z, closestBiome); ;


        Color noiseSum = Color.black;
        for (int i = 0; i < floats.Count; i++)
        {
            floats[i] = (interpDist - floats[i]) / interpDist;

        }
        float floatsSum = floats.Sum();
        for (int i = 0; i < floats.Count; i++)
        {
            noiseSum += (floats[i] / floatsSum) * GetBiomeColor(vert.x, vert.z, closeBiomes[i]);

        }

        return noiseSum;
    }
    public Color GetBiomeColor(float x, float z, BiomeCell biome)
    {
        float sampleX = x / biome.scale;
        float sampleZ = z / biome.scale;

        float y = Mathf.PerlinNoise(sampleX + seed.x, sampleZ + seed.z);
        y = Mathf.Clamp(y, 0f, 1f);

        return biome.gradient.Evaluate(y);
    }
    public GameObject GetPlant(Vector3 vert)
    {
        List<BiomeCell> biomes = GetCurrentBiomes(vert.x, vert.z);
        BiomeCell closestBiome = FindClosestBiome(vert.x, vert.z, biomes);

        if(Random.Range(0f,1f) < closestBiome.foliageMult && closestBiome.foliage.Count > 0)
        {
            return closestBiome.foliage[Random.Range(0, closestBiome.foliage.Count)];
        }
        else return null;
    }
    
    
    BiomeCell FindClosestBiome(float x, float z, List<BiomeCell> biomes)
    {
        BiomeCell closestBiome = null;
        float dist = float.MaxValue;
        foreach(BiomeCell biome in biomes)
        {
            if(closestBiome != null)
            {
                float d = new Vector2(biome.x - x, biome.z - z).magnitude;
                if (d < dist)
                {
                    closestBiome = biome;
                    dist = d;
                }
            }
            else
            {
                closestBiome = biome;
                dist = new Vector2(biome.x - x, biome.z - z).magnitude;
            }

            
        }
        return closestBiome;
    }
    List<BiomeCell> GetCurrentBiomes(float x, float y)
    {
        int size = BiomesGenerator.biomeSize;
        int xIndex = (x >= 0) ? (int)(x / size) : (int)((x / size) - 1);
        int yIndex = (y >= 0) ? (int)(y / size) : (int)((y / size) - 1);
        BiomeCell biome;
        List<BiomeCell> biomes = new List<BiomeCell>();

        for(int i = -1;i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                Vector2Int pos = new Vector2Int((xIndex + i), (yIndex + j));
                if (biomeDict.TryGetValue(pos, out biome))
                {
                    biomes.Add(biome);
                }
                else
                {
                    biomes.Add(new BiomeCell(pos * size, MapManager.biomeData[BiomeType.forest]));
                }
            }
        }
        
        return biomes;
    }
    float GetDist(float x, float z, BiomeCell biome)
    {
        float dist = new Vector2(x - biome.x, z-biome.z).magnitude;
        return dist;
    }
}
