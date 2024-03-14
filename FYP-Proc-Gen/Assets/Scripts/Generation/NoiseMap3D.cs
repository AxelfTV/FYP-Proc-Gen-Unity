using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoiseMap3D
{
    Seed seed;
    float scale = 30f;
    Vector3 centre;
    float boundary = 50;
    float middle = 5;
    public NoiseMap3D(Seed seed, Vector3 centre)
    {
        this.seed = seed;
        this.centre = centre;
    }
    public float GetNoise(Vector3 position)
    {
        if ((position - centre).magnitude < middle) return 0;
        if ((position - centre).magnitude > boundary) return 1;
        return PerlinNoise3D((position.x * 0.9f)/scale + seed.x, (position.y * 0.9f)/scale, (position.z * 0.9f) / scale + seed.z);
    }
    public float GetNoise(float x, float y, float z)
    {
        return PerlinNoise3D((x * 0.9f) / 25 + seed.x, (y * 0.9f) / 25, (z * 0.9f) / 25 + seed.z)*2 -1;
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
