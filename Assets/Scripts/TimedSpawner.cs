using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    public GameObject zombie;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    private void SpawnObject()
    {
        Instantiate(zombie, transform.position, transform.rotation);
        if (stopSpawning)
            CancelInvoke("SpawnObject");
    }
}
