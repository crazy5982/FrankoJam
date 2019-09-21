using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel : MonoBehaviour
{
    // Package weight variable setup
    [SerializeField] int parcelWeight;
	public int ParcelWeight
	{
		get { return parcelWeight; }
	}

	private ScaleZone m_ScaleZone = null;

	public void SetScaleZone(ScaleZone scaleZone)
	{
		m_ScaleZone = scaleZone;
	}

	public bool RemovePackageFromZone(int playerIndex)
	{
		if (m_ScaleZone != null)
		{
			return m_ScaleZone.RemovePackage(this, playerIndex);
		}

		return true;
	}
}