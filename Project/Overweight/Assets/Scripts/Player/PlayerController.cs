using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected float m_MovementSpeed = 0.1f;
	[SerializeField]
	protected float m_DashSpeed = 0.2f;

	[SerializeField]
	protected int m_StunTimeFrames = 90;

	[SerializeField]
	protected int m_DashTimeFrames = 20;
	[SerializeField]
	protected int m_DashCooldownFrames = 180;

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

	private int m_DashCooldown = 0;
	private int m_DashTimer = 0;
	public bool IsDashing
	{
		get { return m_DashTimer > 0; }
	}

	private bool m_IsStunned = false;
	private int m_StunTimer = 0;

	private string m_PlayerNumber;

	private Transform m_AttachPoint;
    private Rigidbody m_RigidBody;
	private Grabber m_Grabber;
	private ParticleSystem m_StunParticles;

	private Rigidbody m_CarriedPackage;

	[SerializeField]
	private AudioClip m_DashClip;
	[SerializeField]
	private AudioClip m_StunClip;

	private AudioSource m_AudioSource;

	private List<int> PLAYER_LAYER_IDS;
	private const int PARCEL_LAYER_ID = 9;
	private const float MAX_TURN_ANGLE = 20.0f * Mathf.Deg2Rad;


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

		m_AudioSource = GetComponent<AudioSource>();
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
					carriedParcel.ThrownIndex = m_PlayerIndex;
				}

				m_CarriedPackage = null;
			}
			else
			{
				if (m_DashCooldown <= 0)
				{
					m_DashTimer = m_DashTimeFrames;
					m_DashCooldown = m_DashCooldownFrames;

					m_AudioSource.clip = m_DashClip;
					m_AudioSource.Play();
				}
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

		if (m_DashTimer > 0)
		{
			m_DashTimer--;
		}

		if (m_DashCooldown > 0)
		{
			m_DashCooldown--;
		}

        Vector2 analogue_input;
        analogue_input.x = Input.GetAxis("Horizontal_P" + m_PlayerNumber);
        analogue_input.y = Input.GetAxis("Vertical_P" + m_PlayerNumber);
        analogue_input *= m_DashTimer > 0 ? m_DashSpeed : m_MovementSpeed;

        if (m_RigidBody && analogue_input.SqrMagnitude() >= 0.00001f)
        {
            Vector3 newPosition = m_RigidBody.position;
            newPosition.x += analogue_input.x;
            newPosition.z += analogue_input.y;
            m_RigidBody.MovePosition(newPosition);

            analogue_input.Normalize();

			//float desiredAngleChange = Vector3.Dot(gameObject.transform.forward, analogue_input);
			//float desiredChangeAbs = Mathf.Abs(desiredAngleChange);
			//if (desiredChangeAbs < MAX_TURN_ANGLE)
			//{
			Vector3 newForwards = Vector3.RotateTowards(gameObject.transform.forward, new Vector3(analogue_input.x, 0.0f, analogue_input.y), MAX_TURN_ANGLE, 0.0f);
			//Quaternion rotation = Quaternion.LookRotation(new Vector3(analogue_input.x, 0.0f, analogue_input.y));
			Quaternion rotation = Quaternion.LookRotation(newForwards);
			m_RigidBody.MoveRotation(rotation);
			//}
			//else
			//{
			//	
			//}
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		parcel collidingParcel = collision.gameObject.GetComponent<parcel>();
		if (collidingParcel != null && collidingParcel.WasThrown)
		{
			if (collidingParcel.ThrownIndex != m_PlayerIndex)
			{
				collidingParcel.ThrownIndex = 0;

				Stun();
			}
		}

		PlayerController collidingPlayer = collision.gameObject.GetComponent<PlayerController>();
		if (collidingPlayer != null && collidingPlayer.IsDashing)
		{
			Stun();
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

	private void Stun()
	{
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

		m_AudioSource.clip = m_StunClip;
		m_AudioSource.Play();
	}
}
