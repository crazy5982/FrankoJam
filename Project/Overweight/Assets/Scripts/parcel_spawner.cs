using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel_spawner : MonoBehaviour
{
    private float nextSpawnTime;
    private float spawnCount = 0;
    private bool canSpawn = false;

    [SerializeField] private GameObject [] parcel_spawnItem;
    [SerializeField] private float spawnDelay = 5;

    private float spawnAve = 5;

    private List<int> parcelSpawnWeight = new List<int>();

    private void Update()
    {
        if (canSpawn)
        {
            if (ShouldSpawn())
            {
                Spawn(parcelToSpawn());
            }
        }
    }

    public void StartSpawning(int numSpawnMin, int numSpawnMax, int smallP, int medP, int largeP, int badP)
    {
        spawnAve = Mathf.RoundToInt(Random.Range(numSpawnMin, numSpawnMax+0.49f));
        parcelSpawnWeight.Add(smallP);
        parcelSpawnWeight.Add(medP);
        parcelSpawnWeight.Add(largeP);
        parcelSpawnWeight.Add(badP);
        canSpawn = true;
    }

    private void Spawn(GameObject parcelToSpawn)
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(parcelToSpawn, transform.position, transform.rotation);
        spawnCount++;
    }

    private bool ShouldSpawn()
    {
        return TimerCheck();
    }

    private bool TimerCheck()
    {
        return Time.time > nextSpawnTime && spawnCount < spawnAve;
    }

    private GameObject parcelToSpawn()
    {
        float choice = Random.Range(0, totalWeighting());
        if(choice >= 0 && choice < parcelSpawnWeight[0])
        {
            return parcel_spawnItem[0];
        }
        else if (choice >= parcelSpawnWeight[0] && choice < givenWeighting(2))
        {
            return parcel_spawnItem[1];
        }
        else if (choice >= parcelSpawnWeight[1] && choice < givenWeighting(3))
        {
            return parcel_spawnItem[2];
        }
        else if (choice >= parcelSpawnWeight[2] && choice < givenWeighting(4))
        {
            return parcel_spawnItem[3];
        }
        else
        {
            return parcel_spawnItem[0];
        }
    }

    private float totalWeighting()
    {
        float tSum = 0f;
        foreach (int parcel in parcelSpawnWeight)
        {
            tSum += parcel;
        }
        return tSum;
    }

    private float givenWeighting(int parcelNum)
    {
        float tSum = 0f;
        for (int i = 0; i < parcelNum; i++)
        {
            tSum += parcelSpawnWeight[i];
        }
        return tSum;
    }
}
