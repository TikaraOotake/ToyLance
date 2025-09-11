using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // �� ��ũ��Ʈ ���� �̱������� ���龁E ���� �ٲ���ѵ �ı�����E�ʰ�E�� �ϳ��� �����ϰ� �մϴ�.
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
        // ���� Ű�е�E6���� ���ȴ���EȮ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            // BGMManager�� ������ ���� ���Ұ� �Լ��� ȣ���մϴ�.
            if (BGMManager.instance != null)
            {
                BGMManager.instance.ToggleBGM();
            }
        }
    }
}
