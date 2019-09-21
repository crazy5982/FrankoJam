using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parcel : MonoBehaviour
{
    // Package weight variable setup
    [SerializeField] int parcelWeight;

    [SerializeField]
    protected float m_ThrowSpeedH = 0.50f;
    [SerializeField]
    protected float m_ThrowSpeedV = 0.10f;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 throwVector = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        rb.velocity = throwVector;
    }
}
