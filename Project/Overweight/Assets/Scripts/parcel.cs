using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class parcel : MonoBehaviour
{
	// Package weight variable setup
	[SerializeField] int parcelWeight;

	private GameObject parcel_ting;

	public int ParcelWeight
	{
		get { return parcelWeight; }
	}

	private ScaleZone m_ScaleZone = null;

	[SerializeField]
	protected float m_ThrowSpeedH = 0.50f;
	[SerializeField]
	protected float m_ThrowSpeedV = 0.10f;

	public Rigidbody rb;

	private bool m_WasThrown = false;
	public bool WasThrown
	{
		get { return m_WasThrown; }
		set { m_WasThrown = value; }
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		//Vector3 throwVector = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
		//rb.velocity = throwVector;
	}

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

	void OnCollisionEnter(Collision collision)
	{
		if (m_WasThrown)
		{
			// Normally want to set was thrown to false on any collision, but if the collision is another player
			// we don't want to reset was thrown before the player gets a chance to react to it!
			// Player will reset it in their OnCollisionEnter function
			PlayerController collidingPlayer = collision.gameObject.GetComponent<PlayerController>();
			if (collidingPlayer == null)
			{
				m_WasThrown = false;
			}
		}
	}
}