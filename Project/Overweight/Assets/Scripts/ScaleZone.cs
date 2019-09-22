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
	public int CurrentWeight
	{
		get { return m_CurrentWeight; }
	}

	private bool m_Enabled = false;
	public bool Enabled
	{
		get { return m_Enabled; }
		set { m_Enabled = value; }
	}

	private List<parcel> m_ParcelList;

    //Bedson's added variables, sorry Dan, plz forgive me
    private TextMesh scaleText;
    private GameObject scaleTextHolder;
    private GameObject evaluationTextHolder;
    private TextMesh evaluationText;
    private int score;
    private int winCount = 0;

	private List<int> PLAYER_LAYER_IDS;
	private int PARCEL_LAYER_ID = 9;

	private void Awake()
	{
		PLAYER_LAYER_IDS = new List<int> { 10, 11, 12, 13 };
	}

	// Start is called before the first frame update
	void Start()
    {
		m_AttachPointParent = transform.GetChild(0);

		m_ParcelList = new List<parcel>(m_AttachPointParent.childCount);
		for (int i = 0; i < m_AttachPointParent.childCount; ++i)
		{
			m_ParcelList.Add(null);
		}

        //Bedson's added bizzle
        scaleTextHolder = this.gameObject.transform.GetChild(1).gameObject;
        scaleText = scaleTextHolder.GetComponent<TextMesh>();
        evaluationTextHolder = this.gameObject.transform.GetChild(2).gameObject;
        evaluationText = evaluationTextHolder.GetComponent<TextMesh>();
        evaluationText.text = "X";
    }

    // Update is called once per frame
    void Update()
    {
        scaleText.text = CurrentWeight + "kg";
    }

	void OnCollisionEnter(Collision collision)
	{
		if (Enabled)
		{
			parcel collidingParcel = collision.gameObject.GetComponent<parcel>();
			if (collidingParcel != null)
			{
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
					parcelRigidBody.gameObject.layer = PLAYER_LAYER_IDS[m_PlayerIndex - 1];
				}
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

		for (int i = 0; i < m_ParcelList.Count; ++i)
		{
			if (m_ParcelList[i] == package)
			{
				m_ParcelList[i] = null;
				break;
			}
		}

		m_CurrentWeight -= package.ParcelWeight;

		package.SetScaleZone(null);

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

		m_CurrentWeight = 0;
	}

    //Bedson's added bizzle

    public void SetEvaluationText(string text)
    {
        evaluationText.text = text;
    }

    public void SetEvaluationTextColour(UnityEngine.Color colour)
    {
        evaluationText.color = colour;
    }

    public string GetEvaluationText()
    {
        return evaluationText.text;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int scoreVal)
    {
        score = scoreVal;
    }

    public int GetWinCount()
    {
        return winCount;
    }
    public void SetWinCount(int winVal)
    {
        winCount = winVal;
    }

    public void AddWins(int winVal)
    {
        winCount = winCount + winVal;
    }

}
