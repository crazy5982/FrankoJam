using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{   
    public enum Scene
    {
        Action_shot,
        Level_1,
        Level_2
    }

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadNextScene()
    {
        int scenes = SceneManager.sceneCount;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < scenes)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public static void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
