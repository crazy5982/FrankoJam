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

    [SerializeField] private int[] spawnWeights;

    Random rand = new Random();

    private void Update()
    {
        if (ShouldSpawn())
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        int spawnNum;
        Instantiate(parcel_spawnItem[Mathf.RoundToInt(Random.Range(0,3.49f))], transform.position, transform.rotation);
        spawnCount++;
    }

    private bool ShouldSpawn()
    {
        return Time.time > nextSpawnTime && spawnCount < spawnMax;
    }

    //private int SpawnedWeight()
    //{
    //    print(ArrayList.Sort(spawnWeights));
    //    return 1;
    //}
}
