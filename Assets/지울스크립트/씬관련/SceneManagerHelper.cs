using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerHelper : MonoBehaviour
{
    public static bool IsSwitchingScene { get; private set; } = false;

    private void Awake()
    {
        IsSwitchingScene = false;
    }

    // 이 함수를 씬 전환 직전에 호출합니다.
    public static void NotifySwitchingScene()
    {
        IsSwitchingScene = true;
    }

    // OnDestroy는 씬 전환 시에도 호출되므로 여기에 추가해도 좋습니다.
    private void OnDestroy()
    {
        IsSwitchingScene = true;
    }
}
