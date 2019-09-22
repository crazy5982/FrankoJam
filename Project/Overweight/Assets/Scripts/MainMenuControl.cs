using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private GameObject imageDisplay;
    [SerializeField] private Texture controlImage;
    [SerializeField] private float mainMenuDelay = 1f;
    [SerializeField] private float controlDisplayDelay = 4f;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("DelayOnMainMenu", mainMenuDelay);
    }

    IEnumerator DelayOnMainMenu(float count)
    {
        yield return new WaitForSeconds(count);
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
            imageDisplay.GetComponent<RawImage>().texture = controlImage;
            StartCoroutine("DelayNextScene", controlDisplayDelay);
        }
    }

    IEnumerator DelayNextScene(float count)
    {
        yield return new WaitForSeconds(count);
        SceneLoader.LoadNextScene();
    }
}
