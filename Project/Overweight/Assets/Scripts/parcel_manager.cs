using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel_manager : MonoBehaviour
{
    // Setup parcel gameObject array
    GameObject[] parcelObjects;
    bool gotParcel_spawns = false;

    public void GetParcelSpawners()
    {
        parcelObjects = GameObject.FindGameObjectsWithTag("Parcel_spawner");
        gotParcel_spawns = true;
    }

    public void DestroyParcels(string tag)
    {
        GameObject[] parcelObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject parcel in parcelObjects)
        {
            GameObject.Destroy(parcel, 0.5f);
        }
    }

    public void BeginParcelSpawning()
    {
        Debug.Log("Inside parcel Spawning start");
        if(!gotParcel_spawns)
        {
            GetParcelSpawners();
        }        
        foreach (GameObject parcel in parcelObjects)
        {
            parcel.GetComponent<parcel_spawner>().StartSpawning();
        }
    }

    public void BeginParcelSpawning(int boxMin, int boxMax, float sBox, float mBox, float lBox, float bBox)
    {
        if (!gotParcel_spawns)
        {
            GetParcelSpawners();
        }
        foreach (GameObject parcel in parcelObjects)
        {
            parcel.GetComponent<parcel_spawner>().StartSpawning(boxMin, boxMax, sBox, mBox, lBox, bBox);
        }
    }
}
