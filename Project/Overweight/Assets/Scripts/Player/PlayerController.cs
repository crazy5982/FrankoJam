using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected float m_MovementSpeed = 0.1f;

	[SerializeField]
	protected int m_StunTimeFrames = 90;

	[SerializeField]
	protected float m_ThrowSpeedH = 0.50f;
	[SerializeField]
	protected float m_ThrowSpeedV = 0.10f;

	[SerializeField]
    protected int m_PlayerIndex = 1;
	public int PlayerIndex
	{
		get { return m_PlayerIndex; }
        set { m_PlayerIndex = value; m_PlayerNumber = m_PlayerIndex.ToString(); }
	}

    protected bool m_Ready = false;
    public bool ready
    {
        get { return m_Ready; }
        set { m_Ready = value; }
    }

	private bool m_IsDashing = false;
	private bool m_IsStunned = false;
	private int m_StunTimer = 0;

	private List<int> PLAYER_LAYER_IDS;
	private const int PARCEL_LAYER_ID = 9;

	private string m_PlayerNumber;

	private Transform m_AttachPoint;
    private Rigidbody m_RigidBody;
	private Grabber m_Grabber;
	private ParticleSystem m_StunParticles;

	private Rigidbody m_CarriedPackage;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.freezeRotation = true;

		m_Grabber = GetComponentInChildren<Grabber>();

		Transform sparkChild = transform.GetChild(5);
		if (sparkChild != null)
		{
			m_StunParticles = sparkChild.GetComponentInChildren<ParticleSystem>();
		}

		m_AttachPoint = transform.GetChild(0);

		m_PlayerNumber = m_PlayerIndex.ToString();

		PLAYER_LAYER_IDS = new List<int> { 10, 11, 12, 13 };
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (m_IsStunned)
		{
			return;
		}

		if (Input.GetButtonDown("GrabDrop_P" + m_PlayerNumber))
		{
			if (m_CarriedPackage == false)
			{
				parcel latestPackage = m_Grabber.GetLatestPackage();
				if (latestPackage != null)
				{
					if (latestPackage.RemovePackageFromZone(m_PlayerIndex))
					{
						latestPackage.gameObject.transform.position = m_AttachPoint.position;
						latestPackage.gameObject.transform.rotation = m_AttachPoint.rotation;
						latestPackage.gameObject.transform.SetParent(m_AttachPoint);

						m_CarriedPackage = latestPackage.GetComponent<Rigidbody>();
						if (m_CarriedPackage != null)
						{
							m_CarriedPackage.isKinematic = true;
							m_CarriedPackage.gameObject.layer = PLAYER_LAYER_IDS[m_PlayerIndex - 1];
						}
					}
				}
			}
			else
			{
				DeparentCarriedPackage();
				m_CarriedPackage = null;
			}

		}
		if (Input.GetButtonDown("Throw_P" + m_PlayerNumber))
		{
			if (m_CarriedPackage != null)
			{
				DeparentCarriedPackage();

				Vector3 throwVector = transform.forward;
				throwVector.x *= m_ThrowSpeedH;
				throwVector.z *= m_ThrowSpeedH;
				throwVector.y = m_ThrowSpeedV;
				m_CarriedPackage.AddForce(throwVector);

				parcel carriedParcel = m_CarriedPackage.GetComponent<parcel>();
				if (carriedParcel)
				{
					carriedParcel.WasThrown = true;
				}

				m_CarriedPackage = null;
			}
		}
    }

    void FixedUpdate()
    {
		m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.angularVelocity = Vector3.zero;

		if (m_IsStunned)
		{
			m_StunTimer--;
			if (m_StunTimer <= 0)
			{
				m_IsStunned = false;
				if (m_StunParticles != null)
				{
					m_StunParticles.Stop();
				}
			}
			return;
		}

        Vector2 analogue_input;
        analogue_input.x = Input.GetAxis("Horizontal_P" + m_PlayerNumber);
        analogue_input.y = Input.GetAxis("Vertical_P" + m_PlayerNumber);
        analogue_input *= m_MovementSpeed;

        if (m_RigidBody && analogue_input.SqrMagnitude() >= 0.00001f)
        {
            Vector3 newPosition = m_RigidBody.position;
            newPosition.x += analogue_input.x;
            newPosition.z += analogue_input.y;
            m_RigidBody.MovePosition(newPosition);

            analogue_input.Normalize();

            Quaternion rotation = Quaternion.LookRotation(new Vector3(analogue_input.x, 0.0f, analogue_input.y));
            m_RigidBody.MoveRotation(rotation);
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		parcel collidingParcel = collision.gameObject.GetComponent<parcel>();
		if (collidingParcel != null && collidingParcel.WasThrown)
		{
			collidingParcel.WasThrown = false;

			DeparentCarriedPackage();
			m_CarriedPackage = null;

			m_StunTimer = m_StunTimeFrames;
			m_IsStunned = true;
			if (m_StunParticles != null)
			{
				m_StunParticles.Play();
				ParticleSystem.EmissionModule module = m_StunParticles.emission;
				module.enabled = true;
			}
		}
	}

	private void DeparentCarriedPackage()
	{
		if (m_CarriedPackage != null)
		{
			m_CarriedPackage.isKinematic = false;
			m_CarriedPackage.gameObject.transform.SetParent(null);

			m_CarriedPackage.gameObject.layer = PARCEL_LAYER_ID;
		}
	}
}
