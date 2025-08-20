using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneReturn : MonoBehaviour
{
    [SerializeField] string gameSceneName = "SampleScene";   // ���� ���� �� �̸�

    void Update()
    {
        if (Input.anyKeyDown) SceneManager.LoadScene(gameSceneName);
    }
}
