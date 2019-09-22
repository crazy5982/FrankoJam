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
        StartCoroutine("DelayOnMainMenu", 1);
    }

    IEnumerator DelayOnMainMenu(float count)
    {
        yield return new WaitForSeconds(count);
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
            SceneLoader.LoadNextScene();
        }
        else if (Input.GetButtonDown("Throw_P" + 1) || Input.GetButtonDown("Throw_P" + 2) || Input.GetButtonDown("Throw_P" + 3) || Input.GetButtonDown("Throw_P" + 4))
        {
            SceneLoader.LoadStartScene();
        }
    }
}
