using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject NPCToSpawn;
    public bool SpawnOnStart = true;
    public GameObject FirstWayPoint;
    // Start is called before the first frame update
    void Start()
    {
        if (SpawnOnStart)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Instantiate(NPCToSpawn, transform.position, Quaternion.identity);
    }
}
