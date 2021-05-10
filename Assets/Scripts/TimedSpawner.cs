using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    public GameObject zombie;
    public List<GameObject> zombies = new List<GameObject>();
    public bool invoke;
    public bool stopSpawn;
    public float spawnTime;
    public float spawnDelay;
    public GameObject[] spawnPoints;

    private void Update()
    {
        if (invoke)
        {
            spawnTime += Time.deltaTime;
            if(spawnTime >= spawnDelay)
            {
                var random = Random.Range(0, spawnPoints.Length + 1);

                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (random == i)
                    {
                        InvokeZombie(i);
                    }
                }
                spawnTime = 0;
            }
        }
        else
        {
            if (!stopSpawn)
            {
                spawnTime += Time.deltaTime;
                if (spawnTime >= spawnDelay)
                {
                    var random = Random.Range(0, zombies.Count);

                    for (int i = 0; i < zombies.Count; i++)
                    {
                        if (random == i)
                        {
                            SetActiveZombie(i);
                        }
                    }
                    spawnTime = 0;
                }
            }
            else
                return;
        }
    }

    void InvokeZombie(int count)
    {
        Instantiate(zombie, spawnPoints[count].transform.position, spawnPoints[count].transform.rotation);
    }

    void SetActiveZombie(int count)
    {
        zombies[count].transform.position = spawnPoints[count].transform.position;
        zombies[count].SetActive(true);
        zombies.RemoveAt(count);
    }
}
