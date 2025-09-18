using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("이동할 씬 설정")]
    [Tooltip("숫자패드 8을 눌렀을 때 이동할 씬의 이름을 정확하게 입력하세요.")]
    public string sceneNameToLoad;

    void Update()
    {
        // 숫자패드 8번 키를 "누르는 첫 순간"을 감지합니다.
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            // 씬 이름이 비어있지 않다면 해당 씬을 불러옵니다.
            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                SceneManager.LoadScene(sceneNameToLoad);
            }
            else
            {
                Debug.LogWarning("이동할 씬 이름이 지정되지 않았습니다!");
            }
        }
    }
}
