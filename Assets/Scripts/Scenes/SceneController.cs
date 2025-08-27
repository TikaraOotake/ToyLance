using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // �� ��ũ��Ʈ�� �̱������� ���龁E ���� �ٲ���ѵ �ı�����E�ʰ�E�� �ϳ��� �����ϰ� �մϴ�.
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
        // ���� Ű�е�E9���� ���ȴ���EȮ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            RestartCurrentScene();
        }
    }

    // ����E���� �ٽ� �ε��ϴ� �Լ�E
    public void RestartCurrentScene()
    {
        // ����EȰ��ȭ�� ���� ������ �����ɴϴ�.
        Scene currentScene = SceneManager.GetActiveScene();

        // ������ �� ������ �̸����� ���� �ٽ� �ε��մϴ�.
        SceneManager.LoadScene(currentScene.name);
    }
}
