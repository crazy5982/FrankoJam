using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel_manager : MonoBehaviour
{
    // Parcel box numbers
    int boxMin = 1;
    int boxMax = 3;

    // Parcel weightings
    float sBox = 1f;
    float mBox = 1f;
    float lBox = 1f;
    float bBox = 1f;

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
        if(!gotParcel_spawns)
        {
            GetParcelSpawners();
        }        
        foreach (GameObject parcel in parcelObjects)
        {
            parcel.GetComponent<parcel_spawner>().StartSpawning(boxMin, boxMax, sBox, mBox, lBox, bBox);
        }
    }
}
