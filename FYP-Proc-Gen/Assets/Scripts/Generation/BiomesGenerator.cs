using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BiomesGenerator
{
    public static int biomeSize = 200;

    public static Dictionary<Vector2Int, BiomeCell> GetBiomes()
    {
        Dictionary<Vector2Int,BiomeCell> biomes = new Dictionary<Vector2Int, BiomeCell>();
        for (int i = -5; i < 6; i++)
        {
            for (int j = -5; j < 6; j++)
            {
                int n = Random.Range(0,MapManager.biomeData.Values.Count);
                Vector2Int pos = new Vector2Int(i, j);
                biomes.Add(pos, new BiomeCell(pos * biomeSize, MapManager.biomeData.Values.ToList()[n]));


            }
        }




        return biomes;
    }
}
