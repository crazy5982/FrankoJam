using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
	List<parcel> m_ObjectList = new List<parcel>();

	[SerializeField] private Material m_GrabMaterial;
	private Material m_PrevMaterial;

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
			if (m_ObjectList.Count > 0)
			{
				int latestIndex = m_ObjectList.Count - 1;
				if (m_ObjectList[latestIndex] != null)
				{
					SetMaterial(m_ObjectList[latestIndex], m_PrevMaterial, false);
				}
			}

			SetMaterial(collidingPackage, m_GrabMaterial, true);

			m_ObjectList.Add(collidingPackage);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		parcel collidingPackage = other.GetComponent<parcel>();
		if (collidingPackage)
		{
			SetMaterial(collidingPackage, m_PrevMaterial, false);

			m_ObjectList.Remove(collidingPackage);
		}
	}

	public parcel GetLatestPackage()
	{
		if (m_ObjectList.Count > 0)
		{
			int latestIndex = m_ObjectList.Count - 1;
			parcel latestPackage = m_ObjectList[latestIndex];

			if (latestPackage != null)
			{
				SetMaterial(latestPackage, m_PrevMaterial, false);
			}

			m_ObjectList.RemoveAt(latestIndex);
			return latestPackage;
		}
		return null;
	}

	private void SetMaterial(parcel package, Material material, bool setPrev)
	{
		MeshRenderer renderer = package.GetComponentInChildren<MeshRenderer>();
		if (renderer != null)
		{
			if (setPrev)
			{
				m_PrevMaterial = renderer.material;
			}

			renderer.material = material;
		}
	}
}
