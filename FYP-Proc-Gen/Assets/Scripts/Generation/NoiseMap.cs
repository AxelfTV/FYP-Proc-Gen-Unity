using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NoiseMap
{
    Seed seed;
    List<Biome> biomes;

    float interpDist = 50;
    public NoiseMap(Seed seed, List<Biome> biomes)
    {
        this.seed = seed;
        this.biomes = biomes;
        
        SetBiomeLocations();
        
    }
    
    public float GetNoise(float x, float z)
    {
        Biome tempBiome = FindClosestBiome(x,z);

        Biome closestBiome = FindClosestBiome(x, z);
        float closestDist = GetDist(x, z, closestBiome);

        List<Biome> closeBiomes = new List<Biome>();
        List<float> floats = new List<float>();
        foreach (Biome biome in biomes)
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
    
    
    float GetBiomeNoise(float x, float z, Biome tempBiome)
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
        Biome closestBiome = FindClosestBiome(vert.x, vert.z);
        float closestDist = GetDist(vert.x, vert.z, closestBiome);

        List<Biome> closeBiomes = new List<Biome>();
        List<float> floats = new List<float>();
        foreach (Biome biome in biomes)
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
    public Color GetBiomeColor(float x, float z, Biome biome)
    {
        float sampleX = x / biome.scale;
        float sampleZ = z / biome.scale;

        float y = Mathf.PerlinNoise(sampleX + seed.x, sampleZ + seed.z);
        y = Mathf.Clamp(y, 0f, 1f);

        return biome.gradient.Evaluate(y);
    }
    public GameObject GetPlant(Vector3 vert)
    {
        Biome closestBiome = FindClosestBiome(vert.x, vert.z);

        if(Random.Range(0f,1f) < closestBiome.foliageMult && closestBiome.foliage.Count > 0)
        {
            return closestBiome.foliage[Random.Range(0, closestBiome.foliage.Count)];
        }
        else return null;
    }
    
    void SetBiomeLocations()
    {
        int radius = 200;

        int i = 0;


        foreach(Biome biome in biomes) 
        {
            if(i == 0)
            {
                biome.x = 100;
                biome.z = 100;

            }
            else
            {
                biome.x = (int)(radius * Mathf.Cos((2 * i * Mathf.PI) / (biomes.Count-1))) + 100;
                biome.z = (int)(radius * Mathf.Sin((2 * i * Mathf.PI) / (biomes.Count-1))) + 100;
                
            }
            
            i++;
        }
    }
    Biome FindClosestBiome(float x, float z)
    {
        Biome closestBiome = null;
        float dist = float.MaxValue;
        foreach(Biome biome in biomes)
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
    /*
    Biome InterpolateBiomes(float x, float z)
    {
        Biome closestBiome = FindClosestBiome(x,z);
        float closestDist = GetDist(x, z, closestBiome);
        float interpDist = 30;

        List<Biome> closeBiomes = new List<Biome>();
        List<float> floats = new List<float>();
        foreach(Biome biome in biomes)
        {
            float distDif = Mathf.Abs(GetDist(x, z, biome) - closestDist);
            if (distDif < interpDist)
            {
                closeBiomes.Add(biome);
                floats.Add(distDif);
            }
        }
        if (closeBiomes.Count <= 1) return closestBiome;

        float t = floats.Sum() / (floats.Count()-1);
        
        for(int i = 0; i < floats.Count; i++)
        {
            floats[i] = (t - floats[i]) / t;
        }
        temp.lacunarity = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            temp.lacunarity += floats[i] * closeBiomes[i].lacunarity;
        }
        temp.persistance = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            temp.persistance += floats[i] * closeBiomes[i].persistance;
        }
        temp.scale = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            temp.scale += floats[i] * closeBiomes[i].scale;
        }
        temp.mult = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            temp.mult += floats[i] * closeBiomes[i].mult;
        }
        temp.heightOffset = 0;
        for (int i = 0; i < floats.Count; i++)
        {
            temp.heightOffset += floats[i] * closeBiomes[i].heightOffset;
        }
        return temp;
    }
    */
    float GetDist(float x, float z, Biome biome)
    {
        float dist = new Vector2(x - biome.x, z-biome.z).magnitude;
        return dist;
    }
}
