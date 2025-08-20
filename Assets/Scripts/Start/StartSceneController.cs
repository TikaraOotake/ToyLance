using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public Text countdownText;      // [Inspector에 드래그] 오른쪽 위 카운트다운 표시용
    private float countdown = 3f;
    private bool isCounting = false;

    void Update()
    {
        if (!isCounting && Input.anyKeyDown)
        {
            isCounting = true;
            countdownText.gameObject.SetActive(true); // UI 보이게
        }

        if (isCounting)
        {
            countdown -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdown).ToString();

            if (countdown <= 0f)
            {
                SceneManager.LoadScene("Stage03");
            }
        }
    }
}
