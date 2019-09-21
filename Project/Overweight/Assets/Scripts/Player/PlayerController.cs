using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected float m_MovementSpeed = 0.1f;

	[SerializeField]
	protected float m_ThrowSpeedH = 0.50f;
	[SerializeField]
	protected float m_ThrowSpeedV = 0.10f;

	[SerializeField]
    protected int m_PlayerIndex = 1;
	public int PlayerIndex
	{
		get { return m_PlayerIndex; }
	}

	private string m_PlayerNumber;

	private Transform m_AttachPoint;
    private Rigidbody m_RigidBody;
	private Grabber m_Grabber;

	private Rigidbody m_CarriedPackage;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.freezeRotation = true;

		m_Grabber = GetComponentInChildren<Grabber>();

		m_AttachPoint = transform.GetChild(0);

		m_PlayerNumber = m_PlayerIndex.ToString();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
						latestPackage.gameObject.transform.SetParent(m_AttachPoint);

						m_CarriedPackage = latestPackage.GetComponent<Rigidbody>();
						if (m_CarriedPackage != null)
						{
							m_CarriedPackage.isKinematic = true;
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

				m_CarriedPackage = null;
			}
		}
    }

    void FixedUpdate()
    {
        m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.angularVelocity = Vector3.zero;

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

	private void DeparentCarriedPackage()
	{
		if (m_CarriedPackage != null)
		{
			m_CarriedPackage.isKinematic = false;
			m_CarriedPackage.gameObject.transform.SetParent(null);
		}
	}
}
