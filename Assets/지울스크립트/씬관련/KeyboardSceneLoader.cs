using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardSceneLoader : MonoBehaviour
{
    [Header("�̵��� �� �̸� ����")]
    [Tooltip("F1 Ű�� ������ �� �̵��� ���� �̸��� �Է��ϼ���.")]
    public string sceneForKeyF1;

    [Tooltip("F2 Ű�� ������ �� �̵��� ���� �̸��� �Է��ϼ���.")]
    public string sceneForKeyF2;

    [Tooltip("F3 Ű�� ������ �� �̵��� ���� �̸��� �Է��ϼ���.")]
    public string sceneForKeyF3;

    [Tooltip("F4 Ű�� ������ �� �̵��� ���� �̸��� �Է��ϼ���.")]
    public string sceneForKeyF4;

    void Update()
    {
        // F1 Ű�� ������ ������ ����
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadSceneByName(sceneForKeyF1);
        }

        // F2 Ű�� ������ ������ ����
        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadSceneByName(sceneForKeyF2);
        }

        // F3 Ű�� ������ ������ ����
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadSceneByName(sceneForKeyF3);
        }

        // F4 Ű�� ������ ������ ����
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LoadSceneByName(sceneForKeyF4);
        }
    }

    // ���� �ҷ����� ���� �Լ� (�ڵ� �ߺ� ����)
    void LoadSceneByName(string sceneName)
    {
        // �� �̸��� ������� �ʴٸ� �ش� ���� �ҷ��ɴϴ�.
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManagerHelper.NotifySwitchingScene();
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("�̵��� �� �̸��� �������� �ʾҽ��ϴ�!");
        }
    }
}
