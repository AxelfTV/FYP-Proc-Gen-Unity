using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BiomeInfo
{
    public static List<BiomeType> plainsList = new List<BiomeType>() 
    { 
        BiomeType.islands,
        BiomeType.mountains,
        BiomeType.plains,
        BiomeType.forest,
        BiomeType.desert
    };
    public static List<BiomeType> desertList = new List<BiomeType>()
    {        
        BiomeType.mountains,
        BiomeType.plains,       
        BiomeType.desert
    };
    public static List<BiomeType> forestList = new List<BiomeType>()
    {      
        BiomeType.plains,
        BiomeType.forest,       
    };
    public static List<BiomeType> mountainsList = new List<BiomeType>()
    {
        
        BiomeType.mountains,
        BiomeType.plains,       
        BiomeType.desert
    };
    public static List<BiomeType> islandsList = new List<BiomeType>()
    {
        BiomeType.islands,
        BiomeType.plains, 
        BiomeType.ocean
    };
    public static List<BiomeType> oceanList = new List<BiomeType>()
    {
        BiomeType.ocean, 
        BiomeType.islands,
    };
    public static List<BiomeType> GetDict(BiomeType type)
    {
        switch (type)
        {
            case BiomeType.plains: return plainsList;
            case BiomeType.islands: return islandsList;
            case BiomeType.desert: return desertList;
            case BiomeType.forest: return forestList;
            case BiomeType.mountains: return mountainsList;
            case BiomeType.ocean: return oceanList;
            default: return null;
        }
    }
}
