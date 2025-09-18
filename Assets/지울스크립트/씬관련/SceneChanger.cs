using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("�̵��� �� ����")]
    [Tooltip("�����е� 8�� ������ �� �̵��� ���� �̸��� ��Ȯ�ϰ� �Է��ϼ���.")]
    public string sceneNameToLoad;

    void Update()
    {
        // �����е� 8�� Ű�� "������ ù ����"�� �����մϴ�.
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            // �� �̸��� ������� �ʴٸ� �ش� ���� �ҷ��ɴϴ�.
            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                SceneManager.LoadScene(sceneNameToLoad);
            }
            else
            {
                Debug.LogWarning("�̵��� �� �̸��� �������� �ʾҽ��ϴ�!");
            }
        }
    }
}
