using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleZone : MonoBehaviour
{
	[SerializeField]
	private int m_PlayerIndex;
	public int PlayerIndex
	{
		get { return m_PlayerIndex; }
	}

	private Transform m_AttachPointParent;

	private int m_CurrentWeight = 0;

	private List<parcel> m_ParcelList;

    // Start is called before the first frame update
    void Start()
    {
		m_AttachPointParent = transform.GetChild(0);

		m_ParcelList = new List<parcel>(m_AttachPointParent.childCount);
		for (int i = 0; i < m_AttachPointParent.childCount; ++i)
		{
			m_ParcelList.Add(null);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnCollisionEnter(Collision collision)
	{
		parcel collidingParcel = collision.gameObject.GetComponent<parcel>();
		if (collidingParcel != null)
		{
			//int thisParcelIndex = m_ParcelList.Count % m_AttachPointParent.childCount;
			int firstFreeIndex = -1;
			for (int i = 0; i < m_AttachPointParent.childCount; ++i)
			{
				if (m_ParcelList[i] == null)
				{
					firstFreeIndex = i;
					break;
				}
			}

			if (firstFreeIndex == -1)
			{
				return;
			}

			// And parcel to the list and increase the weight
			//m_ParcelList.Add(collidingParcel);
			m_ParcelList[firstFreeIndex] = collidingParcel;
			collidingParcel.SetScaleZone(this);

			m_CurrentWeight += collidingParcel.ParcelWeight;

			Transform attachPoint = m_AttachPointParent.GetChild(firstFreeIndex);
			collidingParcel.gameObject.transform.position = attachPoint.position;
			collidingParcel.gameObject.transform.rotation = attachPoint.rotation;

			Rigidbody parcelRigidBody = collidingParcel.GetComponent<Rigidbody>();
			if (parcelRigidBody != null)
			{
				parcelRigidBody.isKinematic = true;
			}
		}
	}

	// Remove package from the zone, and decrease the weight by the amount of the package
	public bool RemovePackage(parcel package, int playerIndex)
	{
		if (playerIndex != m_PlayerIndex)
		{
			return false;
		}

		//m_ParcelList.Remove(package);

		for (int i = 0; i < m_ParcelList.Count; ++i)
		{
			if (m_ParcelList[i] == package)
			{
				m_ParcelList[i] = null;
				break;
			}
		}

		m_CurrentWeight -= package.ParcelWeight;

		return true;
	}

	public void ResetScale()
	{
		for (int i = 0; i < m_ParcelList.Count; ++i)
		{
			if (m_ParcelList[i] != null)
			{
				Destroy(m_ParcelList[i].gameObject);
				m_ParcelList[i] = null;
			}
		}
		//m_ParcelList.Clear();

		m_CurrentWeight = 0;
	}
}
