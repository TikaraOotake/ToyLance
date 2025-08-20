using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.

public class TitleManager : MonoBehaviour
{
    [Header("설정")]
    // Inspector에서 애니메이션을 재생할 Animator를 연결합니다.
    public Animator titleAnimator;

    // Inspector에서 애니메이션이 끝난 후 넘어갈 씬의 이름을 입력합니다.
    public string nextSceneName;

    // 애니메이션 Trigger의 이름을 Inspector에서 설정합니다.
    public string animationTriggerName = "Start";

    // 키 입력이 가능한 상태인지 확인하는 변수입니다.
    private bool canPressKey = true;

    void Update()
    {
        // 키 입력이 가능한 상태이고, 아무 키나 눌렸을 때
        if (canPressKey && Input.anyKeyDown)
        {
            // 키 입력을 비활성화하여 중복 실행을 방지합니다.
            canPressKey = false;

            // Animator가 연결되어 있다면, 설정된 이름의 Trigger를 실행합니다.
            if (titleAnimator != null)
            {
                titleAnimator.SetTrigger(animationTriggerName);
            }
        }
    }

    // ★ 애니메이션 이벤트에서 호출할 함수입니다.
    // 이 함수는 애니메이션 클립의 마지막 프레임에 추가해야 합니다.
    public void OnAnimationFinished()
    {
        // 0.5초 뒤에 LoadNextScene 함수를 실행하도록 예약합니다.
        Invoke("LoadNextScene", 0.5f);
    }

    // 다음 씬을 불러오는 함수입니다.
    private void LoadNextScene()
    {
        // Inspector에서 설정한 이름의 씬을 불러옵니다.
        SceneManager.LoadScene(nextSceneName);
    }
}
