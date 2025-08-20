using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // 이 스크립트 또한 싱글톤으로 만들어, 씬이 바뀌어도 파괴되지 않고 단 하나만 존재하게 합니다.
    public static SoundController instance;

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
        // 숫자 키패드 6번이 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            // BGMManager가 존재할 때만 음소거 함수를 호출합니다.
            if (BGMManager.instance != null)
            {
                BGMManager.instance.ToggleBGM();
            }
        }
    }
}
