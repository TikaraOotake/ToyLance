using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneReturn : MonoBehaviour
{
    [SerializeField] string gameSceneName = "SampleScene";   // 메인 게임 씬 이름

    void Update()
    {
        if (Input.anyKeyDown) SceneManager.LoadScene(gameSceneName);
    }
}
