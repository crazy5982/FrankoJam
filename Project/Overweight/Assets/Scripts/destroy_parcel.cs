using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_parcel : MonoBehaviour
{
    public void DestroyParcels(string tag)
    {
        GameObject[] parcelObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject parcel in parcelObjects)
        {
            GameObject.Destroy(parcel, 0.5f);
        }
    }
}
