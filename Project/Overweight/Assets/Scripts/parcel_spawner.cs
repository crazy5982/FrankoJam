using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel_spawner : MonoBehaviour
{
    private float nextSpawnTime;
    private float spawnCount = 0;
    private bool canSpawn = false;

    [SerializeField] private GameObject [] parcel_spawnItem;
    [SerializeField] private float spawnDelay = 1;
    [SerializeField] private float spawnDelayVariance = 1f;

    private float spawnAve = 5;

    [SerializeField] private string parcelDirection;
    [SerializeField] private float parcelInitalForce = 1.3f;
    [SerializeField] private float parcelForceVariance = 1f;
    [SerializeField] private float parcelSidewaysForce = 1f;

    // Parcel box numbers
    [SerializeField] int boxMin = 1;
    [SerializeField] int boxMax = 3;

    // Parcel weightings
    [SerializeField] float sBox = 1f;
    [SerializeField] float mBox = 1f;
    [SerializeField] float lBox = 1f;
    [SerializeField] float bBox = 1f;

    private List<float> parcelSpawnWeight = new List<float> { 1f , 1f , 1f , 1f };

    private void Update()
    {
        if (canSpawn)
        {
            if (psTimerCheck())
            {
                Spawn(parcelToSpawn());
            }
        }
    }

    public void StartSpawning()
    {
        Debug.Log("parcel spawner");
        spawnAve = Mathf.RoundToInt(Random.Range(boxMin, boxMax + 0.49f));
        parcelSpawnWeight.Clear();
        parcelSpawnWeight.Add(sBox);
        parcelSpawnWeight.Add(mBox);
        parcelSpawnWeight.Add(lBox);
        parcelSpawnWeight.Add(bBox);
        canSpawn = true;
        spawnCount = 0;
    }

    public void StartSpawning(int numSpawnMin, int numSpawnMax, float smallP, float medP, float largeP, float badP)
    {
        spawnAve = Mathf.RoundToInt(Random.Range(numSpawnMin, numSpawnMax+0.49f));
        parcelSpawnWeight.Clear();
        parcelSpawnWeight.Add(smallP);
        parcelSpawnWeight.Add(medP);
        parcelSpawnWeight.Add(largeP);
        parcelSpawnWeight.Add(badP);
        canSpawn = true;
        spawnCount = 0;
    }

    public void SetParcelDirection(string directionP)
    {
        parcelDirection = directionP;
    }

    private void Spawn(GameObject parcelToSpawn)
    {
		if (parcelToSpawn == null)
		{
			return;
		}

        float pfv = Random.Range(parcelInitalForce - parcelForceVariance, parcelInitalForce + parcelForceVariance);
        float psv = Random.Range(-parcelSidewaysForce, parcelSidewaysForce);
        nextSpawnTime = Time.time + Random.Range(spawnDelay - spawnDelayVariance, spawnDelay + spawnDelayVariance);
        GameObject spawnedParcel = Instantiate(parcelToSpawn, transform.position, transform.rotation);
        if (parcelDirection == "right")
        {
            spawnedParcel.GetComponent<Rigidbody>().velocity = new Vector3(pfv, 0, psv);
        }
        else if (parcelDirection == "left")
        {
            spawnedParcel.GetComponent<Rigidbody>().velocity = new Vector3(-pfv, 0, psv);
        }
        else if (parcelDirection == "down")
        {
            spawnedParcel.GetComponent<Rigidbody>().velocity = new Vector3(psv, 0, -pfv);
        }
        else if (parcelDirection == "up")
        {
            spawnedParcel.GetComponent<Rigidbody>().velocity = new Vector3(psv, 0, pfv);
        }
        else
        {
            spawnedParcel.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        spawnCount++;

		AudioSource audio = GetComponent<AudioSource>();
		if (audio != null)
		{
			audio.Play();
		}
    }

    private bool psTimerCheck()
    {
        return Time.time > nextSpawnTime && spawnCount < spawnAve;
    }

    private GameObject parcelToSpawn()
    {
		if (parcelSpawnWeight.Count == 0)
		{
			return null;
		}

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
