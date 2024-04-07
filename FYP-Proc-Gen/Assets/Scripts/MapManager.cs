using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public static Seed seed;
    public static NoiseMap noiseMap;
    public static int xSize;
    public static int zSize;

    [SerializeField] int size;
    [SerializeField] GameObject chunkObj;
    [SerializeField] List<Biome> biomes;
    [SerializeField] Transform player;
     

    Dictionary<Vector2Int, Chunk> chunks;

    
    List<Location> locations;

    public GameObject test;
    public GameObject water;
    void Awake()
    {
        xSize = size;
        zSize = size;
        seed = new Seed();
        noiseMap = new NoiseMap(seed, biomes);
        chunks = new Dictionary<Vector2Int, Chunk>();
        locations = new List<Location>();
    }
    void Start()
    {
        //locations.Add(new TestLocation(new Vector2Int(100, 100), test));
        //locations.Add(new Road(new Vector2Int(50, 50), new Vector2Int(100, 100), Color.gray));
        locations.Add(new River(new Vector2Int(200, 200), new Vector2Int(100, 100), water));


        StartCoroutine(GenerateChunk(Vector2Int.zero));

        


    }
    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            

        }
        ChunkLoad();
    }
    
    void RenderAllLocations()
    {
        foreach(var location in locations)
        {
            Chunk locationChunk;
            foreach(var chunk in location.chunks)
            {
                if (chunks.TryGetValue(chunk, out locationChunk))
                {

                    locationChunk.RenderLocation(location);
                }
            }
        }
    }
    IEnumerator GenerateChunk(Vector2Int pos)
    {
        if(!chunks.TryGetValue(pos, out Chunk test))
        {
            Chunk newChunk = Instantiate(chunkObj, new Vector3(pos.x * size, 0, pos.y * size), Quaternion.identity).GetComponent<Chunk>();
            newChunk.initX = pos.x * size;
            newChunk.initZ = pos.y * size;
            newChunk.index = pos;
            chunks.Add(pos, newChunk);
        }
        yield return new WaitForEndOfFrame();
        RenderAllLocations();
    }
    IEnumerator DeleteChunk(Vector2Int pos)
    {
        yield return new WaitForEndOfFrame();
        Chunk checkChunk;
        if (chunks.TryGetValue(pos, out checkChunk))
        {

            chunks.Remove(pos);
            Destroy(checkChunk.gameObject);
        }
    }
    void ChunkLoad()
    {
        Vector2Int currentChunk = ChunkFuncs.ObjectCurrentChunk(player.position);
        List<Vector2Int> nearbyChunks = new List<Vector2Int>();
        
        float radius = size * 4f;
        
        
        nearbyChunks.Add(currentChunk);
        for (int x = -4; x < 5; x++)
        {
            for(int z = -4; z < 5; z++)
            {
                if (!(x == 0 && z == 0) && ContainedInCircle(currentChunk + new Vector2Int(x, z), radius, player.position - new Vector3(size / 2f, 0, size / 2f))) nearbyChunks.Add(currentChunk + new Vector2Int(x, z));
            }
        }

        foreach (var chunk in chunks)
        {
            if (!nearbyChunks.Contains(chunk.Value.index))
            {
                StartCoroutine(DeleteChunk(chunk.Key));
            }
        }
        foreach(var current in nearbyChunks)
        {
            Chunk checkChunk;
            if (!chunks.TryGetValue(current, out checkChunk))
            {
                StartCoroutine(GenerateChunk(current));
                
            }
        }
    }
    
    bool ContainedInCircle(Vector2Int point, float d, Vector3 centre)
    {
        
        if ((new Vector2(point.x * size, point.y * size) - new Vector2(centre.x, centre.z)).magnitude < d) return true;

        return false;

    }
}
