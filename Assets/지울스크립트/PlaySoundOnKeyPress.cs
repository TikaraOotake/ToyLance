using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ★★★ 씬 관리를 위해 추가 ★★★
// 이 스크립트를 넣으면 AudioSource가 자동으로 추가됩니다.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnKeyPress : MonoBehaviour
{
    [Header("사운드 설정")]
    [Tooltip("재생할 오디오 클립입니다.")]
    public AudioClip soundToPlay;

    [Tooltip("사운드의 크기입니다. (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("쿨타임 설정")]
    [Tooltip("사운드가 다시 재생되기까지 필요한 시간(초)입니다.")]
    public float cooldownTime = 1.0f;

    // ★★★ 추가된 부분 시작 ★★★
    [Header("씬 시작 시 소리 끔 설정")]
    [Tooltip("이 씬이 시작될 때 아래 설정된 시간 동안 소리를 끔깁니다.")]
    public string sceneNameToMute;

    [Tooltip("씬이 시작된 후 소리가 나지 않을 시간(초)입니다.")]
    public float initialMuteDuration = 15.0f;
    // ★★★ 추가된 부분 끝 ★★★


    // --- 내부 변수 ---
    private AudioSource audioSource;
    private float lastPlayTime = -1f;
    private bool isMutedBySceneStart = false; // 씬 시작으로 인해 소리가 꺼졌는지 확인

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;

        // ★★★ 추가된 부분 ★★★
        // 현재 씬의 이름이 인스펙터에서 지정한 씬 이름과 같은지 확인
        if (SceneManager.GetActiveScene().name == sceneNameToMute)
        {
            // 같다면, 설정된 시간 동안 소리를 끄는 코루틴을 시작
            StartCoroutine(InitialMuteCoroutine());
        }
    }

    // ★★★ 추가된 코루틴 ★★★
    IEnumerator InitialMuteCoroutine()
    {
        // 1. 씬 시작 시 소리가 나지 않도록 상태를 변경
        isMutedBySceneStart = true;

        // 2. 인스펙터에서 설정한 시간만큼 기다림
        yield return new WaitForSeconds(initialMuteDuration);

        // 3. 시간이 지나면 다시 소리가 날 수 있도록 상태를 원상 복구
        isMutedBySceneStart = false;
    }

    void Update()
    {
        // ★★★ 변경된 부분 ★★★
        // 입력이 있고 "그리고" 씬 시작으로 인해 소리가 꺼진 상태가 "아닐 때"만
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("joystick button 5")) && !isMutedBySceneStart)
        {
            // 쿨타임 확인 로직
            if (Time.time >= lastPlayTime + cooldownTime)
            {
                lastPlayTime = Time.time;
                if (soundToPlay != null)
                {
                    audioSource.PlayOneShot(soundToPlay, volume);
                }
            }
        }
    }
}
