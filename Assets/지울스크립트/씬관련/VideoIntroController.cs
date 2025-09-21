using UnityEngine;
using UnityEngine.UI; // UI(Image)ｸｦ ﾁｦｾ鏞ﾏｱ・ﾀｧﾇﾘ ﾇﾊｼ・
using UnityEngine.Video; // VideoPlayerｸｦ ﾁｦｾ鏞ﾏｱ・ﾀｧﾇﾘ ﾇﾊｼ・
using UnityEngine.SceneManagement; // Sceneﾀｻ ﾀ・ｯﾇﾏｱ・ﾀｧﾇﾘ ﾇﾊｼ・

public class VideoIntroController : MonoBehaviour
{
    [Header("ﾇﾙｽﾉ ﾄﾄﾆﾍﾆｮ ｿｬｰ・")]
    [Tooltip("ﾀ蟶鯀｡ ﾀﾖｴﾂ ｺﾀ ﾇﾃｷｹﾀﾌｾ・ﾄﾄﾆﾍﾆｮｸｦ ｿｬｰ睇ﾏｼｼｿ・")]
    public VideoPlayer videoPlayer;

    [Tooltip("ｽｺﾅｵ ﾁ犢・ｻ ﾇ･ｽﾃﾇﾒ ｿ・ﾀﾌｹﾌﾁ・UIｸｦ ｿｬｰ睇ﾏｼｼｿ・")]
    public Image skipProgressUI;

    [Header("ｾﾀ ｹﾗ ｽｺﾅｵ ｼｳﾁ､")]
    [Tooltip("ｿｵｻﾌ ｳ｡ｳｭ ﾈﾄ ﾀﾌｵｿﾇﾒ ｾﾀﾀﾇ ﾀﾌｸｧﾀｻ ﾁ､ﾈｮﾇﾏｰﾔ ﾀﾔｷﾂﾇﾏｼｼｿ・")]
    public string nextSceneName;

    [Tooltip("ｽｺﾅｵﾀｻ ﾀｧﾇﾘ ｹｰﾀｻ ｴｩｸ｣ｰ・ﾀﾖｾ﨨ﾟ ﾇﾒ ｽﾃｰ｣(ﾃﾊ)ﾀﾔｴﾏｴﾙ.")]
    public float skipHoldDuration = 2.5f;

    // --- ｳｻｺﾎ ｺｯｼ・---
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

    // ｡ﾚ｡ﾚ｡ﾚ ｺｯｰ豬ﾈ ｺﾎｺﾐ ｽﾃﾀﾛ ｡ﾚ｡ﾚ｡ﾚ
    void Update()
    {
        // ｴﾙﾀｽ ｾﾀﾀｸｷﾎ ｳﾑｾ銧｡ｴﾂ ﾁﾟﾀﾌｸ・ﾀﾔｷﾂﾀｻ ｹﾞﾁ・ｾﾊﾀｽ
        if (isLoadingNextScene) return;

        // 1. ｽｺﾅｵ ｹｰﾀﾌ 'ｴｭｸｮｰ・ﾀﾖｴﾂﾁ・ ｸﾕﾀ・ﾈｮﾀﾎ (ﾆﾐｵ・Yｹｰ ｶﾇｴﾂ ﾅｰｺｸｵ・Yﾅｰ)
        bool isSkipButtonPressed = Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Y);

        // 2. ｹｰﾀﾌ ｴｭｸｮｰ・ﾀﾖｴﾙｸ・ﾅｸﾀﾌｸﾓｸｦ ﾁ｡ｽﾃﾅｰｰ・UIｸｦ ｾ･ﾀﾌﾆｮ
        if (isSkipButtonPressed)
        {
            // UIｰ｡ ｲｨﾁｮﾀﾖｴﾙｸ・ﾄﾔ
            if (skipProgressUI != null && !skipProgressUI.gameObject.activeInHierarchy)
            {
                skipProgressUI.gameObject.SetActive(true);
            }

            holdTimer += Time.deltaTime; // ﾅｸﾀﾌｸﾓ ﾁ｡
            if (skipProgressUI != null)
            {
                // ｿ・UIﾀﾇ ﾃ､ｿ・ｰｪﾀｻ ｾ･ﾀﾌﾆｮ
                skipProgressUI.fillAmount = holdTimer / skipHoldDuration;
            }

            // ﾅｸﾀﾌｸﾓｰ｡ ｼｳﾁ､ｵﾈ ｽﾃｰ｣ﾀｻ ﾃ､ｿ・・ｾﾀ ﾀﾌｵｿ
            if (holdTimer >= skipHoldDuration)
            {
                LoadNextScene();
            }
        }
        else // 3. ｹｰﾀｻ ｴｩｸ｣ｰ・ﾀﾖﾁ・ｾﾊｴﾙｸ・ﾅｸﾀﾌｸﾓｿﾍ UIｸｦ ﾃﾊｱ篳ｭ
        {
            if (holdTimer > 0) // ｺﾒﾇﾊｿ萇ﾑ ｰ霆・ｻ ｸｷｱ・ﾀｧﾇﾘ ﾅｸﾀﾌｸﾓｰ｡ 0ｺｸｴﾙ ﾅｬ ｶｧｸｸ ﾃﾊｱ篳ｭ
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
    // ｡ﾚ｡ﾚ｡ﾚ ｺｯｰ豬ﾈ ｺﾎｺﾐ ｳ｡ ｡ﾚ｡ﾚ｡ﾚ

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
