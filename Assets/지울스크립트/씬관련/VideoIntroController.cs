using UnityEngine;
using UnityEngine.UI; // UI(Image)를 제어하기 위해 필수!
using UnityEngine.Video; // VideoPlayer를 제어하기 위해 필수!
using UnityEngine.SceneManagement; // Scene을 전환하기 위해 필수!

public class VideoIntroController : MonoBehaviour
{
    [Header("핵심 컴포넌트 연결")]
    [Tooltip("장면에 있는 비디오 플레이어 컴포넌트를 연결하세요.")]
    public VideoPlayer videoPlayer;

    [Tooltip("스킵 진행률을 표시할 원형 이미지 UI를 연결하세요.")]
    public Image skipProgressUI;

    [Header("씬 및 스킵 설정")]
    [Tooltip("영상이 끝난 후 이동할 씬의 이름을 정확하게 입력하세요.")]
    public string nextSceneName;

    [Tooltip("스킵을 위해 버튼을 누르고 있어야 할 시간(초)입니다.")]
    public float skipHoldDuration = 2.5f;

    // --- 내부 변수 ---
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

    // ★★★ 변경된 부분 시작 ★★★
    void Update()
    {
        // 다음 씬으로 넘어가는 중이면 입력을 받지 않음
        if (isLoadingNextScene) return;

        // 1. 스킵 버튼이 '눌리고 있는지' 먼저 확인 (패드 Y버튼 또는 키보드 Y키)
        bool isSkipButtonPressed = Input.GetButton("Joystick1Button3") || Input.GetKey(KeyCode.Y);

        // 2. 버튼이 눌리고 있다면 타이머를 증가시키고 UI를 업데이트
        if (isSkipButtonPressed)
        {
            // UI가 꺼져있다면 켬
            if (skipProgressUI != null && !skipProgressUI.gameObject.activeInHierarchy)
            {
                skipProgressUI.gameObject.SetActive(true);
            }

            holdTimer += Time.deltaTime; // 타이머 증가
            if (skipProgressUI != null)
            {
                // 원형 UI의 채움 값을 업데이트
                skipProgressUI.fillAmount = holdTimer / skipHoldDuration;
            }

            // 타이머가 설정된 시간을 채우면 씬 이동
            if (holdTimer >= skipHoldDuration)
            {
                LoadNextScene();
            }
        }
        else // 3. 버튼을 누르고 있지 않다면 타이머와 UI를 초기화
        {
            if (holdTimer > 0) // 불필요한 계산을 막기 위해 타이머가 0보다 클 때만 초기화
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
    // ★★★ 변경된 부분 끝 ★★★

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
