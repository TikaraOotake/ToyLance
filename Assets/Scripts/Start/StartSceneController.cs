using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public Text countdownText;      // [Inspector�� �巡��] ������ �� ī��⮴ٿ�Eǥ�ÿ�E
    private float countdown = 3f;
    private bool isCounting = false;

    void Update()
    {
        if (!isCounting && Input.anyKeyDown)
        {
            isCounting = true;
            countdownText.gameObject.SetActive(true); // UI ���̰�
        }

        if (isCounting)
        {
            countdown -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdown).ToString();

            if (countdown <= 0f)
            {
                SceneManager.LoadScene("Stage01");
            }
        }
    }
}
