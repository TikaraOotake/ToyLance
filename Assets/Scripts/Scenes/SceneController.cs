using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 이 스크립트는 싱글톤으로 만들어, 씬이 바뀌어도 파괴되지 않고 단 하나만 존재하게 합니다.
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
        // 숫자 키패드 9번이 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            RestartCurrentScene();
        }
    }

    // 현재 씬을 다시 로드하는 함수
    public void RestartCurrentScene()
    {
        // 현재 활성화된 씬의 정보를 가져옵니다.
        Scene currentScene = SceneManager.GetActiveScene();

        // 가져온 씬 정보의 이름으로 씬을 다시 로드합니다.
        SceneManager.LoadScene(currentScene.name);
    }
}
