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

    // �� �Լ��� �� ��ȯ ������ ȣ���մϴ�.
    public static void NotifySwitchingScene()
    {
        IsSwitchingScene = true;
    }

    // OnDestroy�� �� ��ȯ �ÿ��� ȣ��ǹǷ� ���⿡ �߰��ص� �����ϴ�.
    private void OnDestroy()
    {
        IsSwitchingScene = true;
    }
}
