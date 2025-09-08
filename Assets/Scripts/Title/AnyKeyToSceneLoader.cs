using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToSceneLoader : MonoBehaviour
{
    [Tooltip("SceneName")]
    public string sceneNameToLoad;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
