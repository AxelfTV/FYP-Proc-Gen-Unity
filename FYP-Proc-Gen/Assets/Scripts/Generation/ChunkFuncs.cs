using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkFuncs
{
    public static List<Vector2Int> CheckChunks(List<Vector2Int> vertices)
    {
        List<Vector2Int> chunks = new List<Vector2Int>();

        foreach (var position in vertices)
        {
            List<Vector2Int> chunksTemp = GetChunkList(position);
            foreach(var chunk in chunksTemp)
            {
                if (!chunks.Contains(chunk)) chunks.Add(chunk);
            }
            

        }
        return chunks;
    }
    public static bool IsContainedInChunk(Vector2Int vertex, Vector2Int chunkIndex)
    {

        if(GetChunkList(vertex).Contains(chunkIndex)) return true;
        
        return false;
    }
    public static int GetChunkPosition(int i)
    {
        if (i >= 0) return (int)(i / MapManager.xSize);
        else return (int)(i / MapManager.xSize) - 1;
    }
    public static Vector2Int GetChunkPosition(Vector2Int vertex)
    {
        Vector2Int chunk = Vector2Int.zero;
        if (vertex.x >= 0) chunk.x = (int)(vertex.x / MapManager.xSize);
        else chunk.x = (int)(vertex.x / MapManager.xSize) - 1;
        if (vertex.y >= 0) chunk.y = (int)(vertex.y / MapManager.xSize);
        else chunk.y = (int)(vertex.y / MapManager.xSize) - 1;

        return chunk;
    }
    public static List<Vector2Int> GetChunkList(Vector2Int vertex)
    {
        List<Vector2Int> chunks = new List<Vector2Int>();
        Vector2Int chunk = GetChunkPosition(vertex);
        chunks.Add(chunk);
        if (mod(vertex.x, MapManager.xSize) == 0 && vertex.x >= 0) chunks.Add(chunk - Vector2Int.right);
        if (mod(vertex.y, MapManager.xSize) == 0 && vertex.y >= 0) chunks.Add(chunk - Vector2Int.up);
        if (mod(vertex.x, MapManager.xSize) == 0 && vertex.x < 0) chunks.Add(chunk + Vector2Int.right);
        if (mod(vertex.y, MapManager.xSize) == 0 && vertex.y < 0) chunks.Add(chunk + Vector2Int.up);
        if (mod(vertex.x, MapManager.xSize) == 0 && mod(vertex.y, MapManager.xSize) == 0 && vertex.x >= 0 && vertex.y >= 0) chunks.Add(chunk - Vector2Int.right - Vector2Int.up);
        if (mod(vertex.x, MapManager.xSize) == 0 && mod(vertex.y, MapManager.xSize) == 0 && vertex.x < 0 && vertex.y >= 0) chunks.Add(chunk + Vector2Int.right - Vector2Int.up);
        if (mod(vertex.x, MapManager.xSize) == 0 && mod(vertex.y, MapManager.xSize) == 0 && vertex.x >= 0 && vertex.y < 0) chunks.Add(chunk - Vector2Int.right + Vector2Int.up);
        if (mod(vertex.x, MapManager.xSize) == 0 && mod(vertex.y, MapManager.xSize) == 0 && vertex.x < 0 && vertex.y < 0) chunks.Add(chunk + Vector2Int.right + Vector2Int.up);

        return chunks;
    }
    public static int mod(int x, int y)
    {
        return (x % y + y) % y;
    }
    public static int PositionToIndex(Vector2Int position, Vector2Int chunkIndex)
    {
        
        if (!IsContainedInChunk(position, chunkIndex)) return -1;
        int x;
        int z;
        if (ChunkFuncs.mod(position.x, MapManager.xSize) == 0 && position.x/ MapManager.xSize > chunkIndex.x) x = MapManager.xSize;
        else x = ChunkFuncs.mod(position.x, MapManager.xSize);
        if (ChunkFuncs.mod(position.y, MapManager.zSize) == 0 && position.y / MapManager.zSize > chunkIndex.y) z = MapManager.xSize;
        else z = ChunkFuncs.mod(position.y, MapManager.zSize);

        

        return x + z * (MapManager.xSize + 1);
    }
    public static Vector2Int ObjectCurrentChunk(Vector3 position)
    {
        Vector2Int currentChunk = new Vector2Int(GetChunkPosition((int)position.x), GetChunkPosition((int)position.z));
        return currentChunk;
    }
}
