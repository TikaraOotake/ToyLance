using UnityEngine;
using UnityEngine.UI; // UI(Image)�� �����ϱ� ���� �ʼ�!
using UnityEngine.Video; // VideoPlayer�� �����ϱ� ���� �ʼ�!
using UnityEngine.SceneManagement; // Scene�� ��ȯ�ϱ� ���� �ʼ�!

public class VideoIntroController : MonoBehaviour
{
    [Header("�ٽ� ������Ʈ ����")]
    [Tooltip("��鿡 �ִ� ���� �÷��̾� ������Ʈ�� �����ϼ���.")]
    public VideoPlayer videoPlayer;

    [Tooltip("��ŵ ������� ǥ���� ���� �̹��� UI�� �����ϼ���.")]
    public Image skipProgressUI;

    [Header("�� �� ��ŵ ����")]
    [Tooltip("������ ���� �� �̵��� ���� �̸��� ��Ȯ�ϰ� �Է��ϼ���.")]
    public string nextSceneName;

    [Tooltip("��ŵ�� ���� ��ư�� ������ �־�� �� �ð�(��)�Դϴ�.")]
    public float skipHoldDuration = 2.5f;

    // --- ���� ���� ---
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
        // ���� ������ �Ѿ�� ���̸� �Է��� ���� ����
        if (isLoadingNextScene) return;

        // 1. ��ŵ ��ư�� '������ �ִ���' ���� Ȯ�� (�е� Y��ư �Ǵ� Ű���� YŰ)
        bool isSkipButtonPressed = Input.GetButton("Joystick1Button3") || Input.GetKey(KeyCode.Y);

        // 2. ��ư�� ������ �ִٸ� Ÿ�̸Ӹ� ������Ű�� UI�� ������Ʈ
        if (isSkipButtonPressed)
        {
            // UI�� �����ִٸ� ��
            if (skipProgressUI != null && !skipProgressUI.gameObject.activeInHierarchy)
            {
                skipProgressUI.gameObject.SetActive(true);
            }

            holdTimer += Time.deltaTime; // Ÿ�̸� ����
            if (skipProgressUI != null)
            {
                // ���� UI�� ä�� ���� ������Ʈ
                skipProgressUI.fillAmount = holdTimer / skipHoldDuration;
            }

            // Ÿ�̸Ӱ� ������ �ð��� ä��� �� �̵�
            if (holdTimer >= skipHoldDuration)
            {
                LoadNextScene();
            }
        }
        else // 3. ��ư�� ������ ���� �ʴٸ� Ÿ�̸ӿ� UI�� �ʱ�ȭ
        {
            if (holdTimer > 0) // ���ʿ��� ����� ���� ���� Ÿ�̸Ӱ� 0���� Ŭ ���� �ʱ�ȭ
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
