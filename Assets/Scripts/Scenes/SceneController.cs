using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 이 스크립트는 싱글톤으로 만들푳E 씬이 바끔拔祁 파괴되햨E않컖E단 하나만 존재하게 합니다.
    public static SceneController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 숫자 키패탛E9번이 눌렸는햨E확인합니다.
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            RestartCurrentScene();
        }
    }

    // 현픸E씬을 다시 로드하는 함펯E
    public void RestartCurrentScene()
    {
        // 현픸E활성화된 씬의 정보를 가져옵니다.
        Scene currentScene = SceneManager.GetActiveScene();

        // 가져온 씬 정보의 이름으로 씬을 다시 로드합니다.
        SceneManager.LoadScene(currentScene.name);
    }
}
