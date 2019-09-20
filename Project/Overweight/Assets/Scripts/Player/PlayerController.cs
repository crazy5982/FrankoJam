using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float m_MovementSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 analogue_input;
        analogue_input.x = Input.GetAxis("Horizontal");
        analogue_input.y = Input.GetAxis("Vertical");
        analogue_input *= m_MovementSpeed;

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        if (rigidBody)
        {
            Vector3 newPosition = rigidBody.position;
            newPosition.x += analogue_input.x;
            newPosition.z += analogue_input.y;
            rigidBody.MovePosition(newPosition);
        }

        analogue_input.Normalize();

        Quaternion rotation = Quaternion.LookRotation(new Vector3(analogue_input.x, 0.0f, analogue_input.y));
        rigidBody.MoveRotation(rotation);
    }
}
