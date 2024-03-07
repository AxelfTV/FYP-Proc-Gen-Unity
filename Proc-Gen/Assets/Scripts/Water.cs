using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(MapManager.xSize, 0.01f, MapManager.zSize);
        transform.position += new Vector3(MapManager.xSize/2, 0, MapManager.zSize/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
