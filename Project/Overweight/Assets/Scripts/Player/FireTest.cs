using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(new Vector3(5000.0f, 0.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
