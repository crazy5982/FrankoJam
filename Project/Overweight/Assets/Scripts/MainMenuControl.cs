using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
            SceneLoader.LoadNextScene();
        }
    }
}
