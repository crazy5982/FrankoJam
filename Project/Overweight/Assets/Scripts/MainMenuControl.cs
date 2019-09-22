using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private GameObject imageDisplay;
    [SerializeField] private Texture controlImage;
    [SerializeField] private float mainMenuDelay = 1f;
    [SerializeField] private float controlDisplayDelay = 1f;

    private bool controlsVisible = false;

    // Update is called once per frame
    void Update()
    {
        if(mmcTimerCheck() && controlsVisible == false)
        {
            DisplayControls();
        }
        else if (controlsVisible)
        {
            GoNextScene();
        }
    }

    private bool mmcTimerCheck()
    {
        return Time.time > mainMenuDelay;
    }

    private void DisplayControls()
    {
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
            imageDisplay.GetComponent<RawImage>().texture = controlImage;
            controlsVisible = true;
        }
    }

    private void GoNextScene()
    {
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
            SceneLoader.LoadNextScene();
            controlsVisible = false;
        }
    }
}
