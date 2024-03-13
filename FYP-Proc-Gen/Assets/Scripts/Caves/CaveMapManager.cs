using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public static Seed seed;

    // Start is called before the first frame update
    void Awake()
    {
        seed = new Seed();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            seed = new Seed();

        }
    }
}
