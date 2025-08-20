using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // �� ��ũ��Ʈ�� �̱������� �����, ���� �ٲ� �ı����� �ʰ� �� �ϳ��� �����ϰ� �մϴ�.
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
        // ���� Ű�е� 9���� ���ȴ��� Ȯ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            RestartCurrentScene();
        }
    }

    // ���� ���� �ٽ� �ε��ϴ� �Լ�
    public void RestartCurrentScene()
    {
        // ���� Ȱ��ȭ�� ���� ������ �����ɴϴ�.
        Scene currentScene = SceneManager.GetActiveScene();

        // ������ �� ������ �̸����� ���� �ٽ� �ε��մϴ�.
        SceneManager.LoadScene(currentScene.name);
    }
}
