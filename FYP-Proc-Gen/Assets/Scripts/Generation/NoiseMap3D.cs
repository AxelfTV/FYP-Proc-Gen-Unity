using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap3D
{
    Seed seed;
    public NoiseMap3D(Seed seed)
    {
        this.seed = seed;       
    }
    public float GetNoise(Vector3 position)
    {
        return PerlinNoise3D(position.x + seed.x, position.y, position.z + seed.z);
    }
    public float GetNoise(float x, float y, float z)
    {
        return PerlinNoise3D(x/1000f + seed.x, y/10f, z/1000f + seed.z) * 2f -1f;
    }
    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);

        return xy * xz * yz * yx * zx * zy;
    }

    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }
}
