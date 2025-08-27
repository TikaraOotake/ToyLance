using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ��E��� ���� �ʿ��մϴ�.

public class TitleManager : MonoBehaviour
{
    [Header("����")]
    // Inspector���� �ִϸ��̼��� ����� Animator�� �����մϴ�.
    public Animator titleAnimator;

    // Inspector���� �ִϸ��̼��� ���� �� �Ѿ�̥ ���� �̸��� �Է��մϴ�.
    public string nextSceneName;

    // �ִϸ��̼� Trigger�� �̸��� Inspector���� �����մϴ�.
    public string animationTriggerName = "Start";

    // Ű �Է��� ������ ��������EȮ���ϴ� �����Դϴ�.
    private bool canPressKey = true;

    void Update()
    {
        // Ű �Է��� ������ �����̰�E �ƹ� Ű�� ������ ��
        if (canPressKey && Input.anyKeyDown)
        {
            // Ű �Է��� ��Ȱ��ȭ�Ͽ� �ߺ� ������ �����մϴ�.
            canPressKey = false;

            // Animator�� ����Ǿ�E�ִٸ�E ������ �̸��� Trigger�� �����մϴ�.
            if (titleAnimator != null)
            {
                titleAnimator.SetTrigger(animationTriggerName);
            }
        }
    }

    // �� �ִϸ��̼� �̺�Ʈ���� ȣ���� �Լ��Դϴ�.
    // �� �Լ��� �ִϸ��̼� Ŭ���� ������ �����ӿ� �߰��ؾ� �մϴ�.
    public void OnAnimationFinished()
    {
        // 0.5�� �ڿ� LoadNextScene �Լ��� �����ϵ��� �����մϴ�.
        Invoke("LoadNextScene", 0.5f);
    }

    // ���� ���� �ҷ����� �Լ��Դϴ�.
    private void LoadNextScene()
    {
        // Inspector���� ������ �̸��� ���� �ҷ��ɴϴ�.
        SceneManager.LoadScene(nextSceneName);
    }
}
