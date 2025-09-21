using UnityEngine;
using UnityEngine.UI; // UI(Image)�� �����ϱ�E���� �ʼ�E
using UnityEngine.Video; // VideoPlayer�� �����ϱ�E���� �ʼ�E
using UnityEngine.SceneManagement; // Scene�� ��E��ϱ�E���� �ʼ�E

public class VideoIntroController : MonoBehaviour
{
    [Header("�ٽ� ������Ʈ ����E")]
    [Tooltip("��鿡 �ִ� ���� �÷��̾�E������Ʈ�� �����ϼ���E")]
    public VideoPlayer videoPlayer;

    [Tooltip("��ŵ ���ශE� ǥ���� ��ǁE�̹���EUI�� �����ϼ���E")]
    public Image skipProgressUI;

    [Header("�� �� ��ŵ ����")]
    [Tooltip("������ ���� �� �̵��� ���� �̸��� ��Ȯ�ϰ� �Է��ϼ���E")]
    public string nextSceneName;

    [Tooltip("��ŵ�� ���� ��ư�� ������E�־��� �� �ð�(��)�Դϴ�.")]
    public float skipHoldDuration = 2.5f;

    // --- ���� ����E---
    private float holdTimer = 0f;
    private bool isLoadingNextScene = false;

    void Start()
    {
        if (skipProgressUI != null)
        {
            skipProgressUI.gameObject.SetActive(false);
            skipProgressUI.fillAmount = 0;
        }
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // �ڡڡ� ����� �κ� ���� �ڡڡ�
    void Update()
    {
        // ���� ������ �Ѿ�̡�� ���̸�E�Է��� ����E����
        if (isLoadingNextScene) return;

        // 1. ��ŵ ��ư�� '������E�ִ���E ����EȮ�� (�е�EY��ư �Ǵ� Ű����EYŰ)
        bool isSkipButtonPressed = Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Y);

        // 2. ��ư�� ������E�ִٸ�EŸ�̸Ӹ� ������Ű��EUI�� ������Ʈ
        if (isSkipButtonPressed)
        {
            // UI�� �����ִٸ�E��
            if (skipProgressUI != null && !skipProgressUI.gameObject.activeInHierarchy)
            {
                skipProgressUI.gameObject.SetActive(true);
            }

            holdTimer += Time.deltaTime; // Ÿ�̸� ����
            if (skipProgressUI != null)
            {
                // ��ǁEUI�� ä��E���� ������Ʈ
                skipProgressUI.fillAmount = holdTimer / skipHoldDuration;
            }

            // Ÿ�̸Ӱ� ������ �ð��� ä��E�E�� �̵�
            if (holdTimer >= skipHoldDuration)
            {
                LoadNextScene();
            }
        }
        else // 3. ��ư�� ������E����E�ʴٸ�EŸ�̸ӿ� UI�� �ʱ�ȭ
        {
            if (holdTimer > 0) // ���ʿ��� �軁E� ����E���� Ÿ�̸Ӱ� 0���� Ŭ ���� �ʱ�ȭ
            {
                holdTimer = 0f;
                if (skipProgressUI != null)
                {
                    skipProgressUI.fillAmount = 0;
                    skipProgressUI.gameObject.SetActive(false);
                }
            }
        }
    }
    // �ڡڡ� ����� �κ� �� �ڡڡ�

    void OnVideoEnd(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (isLoadingNextScene) return;
        isLoadingNextScene = true;
        videoPlayer.loopPointReached -= OnVideoEnd;
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
