using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed
{
    public float x;
    public float z;
    public int value;

    public Seed()
    {
        x = Random.Range(-10000f, 10000f);
        z = Random.Range(-10000f, 10000f);
        value = (int)(x + z);
    }
    public Seed(float x, float z)
    {
        this.x = x;
        this.z = z;
        this.value = (int)(x + z);
    }
}
