using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardSceneLoader : MonoBehaviour
{
    [Header("이동할 씬 이름 설정")]
    [Tooltip("F1 키를 눌렀을 때 이동할 씬의 이름을 입력하세요.")]
    public string sceneForKeyF1;

    [Tooltip("F2 키를 눌렀을 때 이동할 씬의 이름을 입력하세요.")]
    public string sceneForKeyF2;

    [Tooltip("F3 키를 눌렀을 때 이동할 씬의 이름을 입력하세요.")]
    public string sceneForKeyF3;

    [Tooltip("F4 키를 눌렀을 때 이동할 씬의 이름을 입력하세요.")]
    public string sceneForKeyF4;

    void Update()
    {
        // F1 키를 누르는 순간을 감지
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadSceneByName(sceneForKeyF1);
        }

        // F2 키를 누르는 순간을 감지
        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadSceneByName(sceneForKeyF2);
        }

        // F3 키를 누르는 순간을 감지
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadSceneByName(sceneForKeyF3);
        }

        // F4 키를 누르는 순간을 감지
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LoadSceneByName(sceneForKeyF4);
        }
    }

    // 씬을 불러오는 공용 함수 (코드 중복 방지)
    void LoadSceneByName(string sceneName)
    {
        // 씬 이름이 비어있지 않다면 해당 씬을 불러옵니다.
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManagerHelper.NotifySwitchingScene();
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("이동할 씬 이름이 지정되지 않았습니다!");
        }
    }
}
