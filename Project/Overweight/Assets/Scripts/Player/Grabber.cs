using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
	List<parcel> m_ObjectList = new List<parcel>();
	
	// Start is called before the first frame update
	void Start()
	{
	    
	}
	
	// Update is called once per frame
	void Update()
	{
	    
	}
	
	void OnTriggerEnter(Collider other)
	{
		parcel collidingPackage = other.GetComponent<parcel>();
		if (collidingPackage)
		{
			m_ObjectList.Add(collidingPackage);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		parcel collidingPackage = other.GetComponent<parcel>();
		if (collidingPackage)
		{
			m_ObjectList.Remove(collidingPackage);
		}
	}

	public parcel GetLatestPackage()
	{
		if (m_ObjectList.Count > 0)
		{
			int latestIndex = m_ObjectList.Count - 1;
			parcel latestPackage = m_ObjectList[latestIndex];
			m_ObjectList.RemoveAt(latestIndex);
			return latestPackage;
		}
		return null;
	}
}
