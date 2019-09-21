using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel_spawner : MonoBehaviour
{
    private float nextSpawnTime;
    private float spawnCount = 0;

    [SerializeField] private GameObject [] parcel_spawnItem;
    [SerializeField] private float spawnDelay = 5;
    [SerializeField] private float spawnMax = 5;

    [SerializeField] private int[] parcelSpawnWeight = {2,1,1,1};

    private void Update()
    {
        if (ShouldSpawn())
        {
            Spawn(parcelToSpawn());
        }
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
        return Time.time > nextSpawnTime && spawnCount < spawnMax;
    }

    private GameObject parcelToSpawn()
    {
        float choice = Random.Range(0, totalWeighting());
        print("The Choice is : " + choice);
        if(choice >= 0 && choice < parcelSpawnWeight[0])
        {
            print("Here");
            return parcel_spawnItem[0];
        }
        else if (choice >= parcelSpawnWeight[0] && choice < givenWeighting(2))
        {
            print("There");
            return parcel_spawnItem[1];
        }
        else if (choice >= parcelSpawnWeight[1] && choice < givenWeighting(3))
        {
            print("Be");
            return parcel_spawnItem[2];
        }
        else if (choice >= parcelSpawnWeight[2] && choice < givenWeighting(4))
        {
            print("Monsters");
            return parcel_spawnItem[3];
        }
        else
        {
            print("Man");
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
        print("The tSum is : " + tSum);
        return tSum;
    }
}
