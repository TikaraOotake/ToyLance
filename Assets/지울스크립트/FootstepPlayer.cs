using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // 이 스크립트를 넣으면 AudioSource가 자동으로 추가됩니다.
public class FootstepPlayer : MonoBehaviour
{
    [Header("발소리 오디오 설정")]
    [Tooltip("재생할 발소리 오디오 파일을 순서대로 넣어주세요.")]
    public AudioClip[] footstepSounds;

    [Tooltip("발소리가 나는 시간 간격입니다. (예: 0.5초에 한 번)")]
    [Range(0.1f, 2f)]
    public float footstepInterval = 0.8f;

    [Tooltip("발소리의 크기입니다. (0 ~ 1)")]
    [Range(0f, 1f)]
    public float footstepVolume = 1.0f;

    [Header("점프 설정")]
    [Tooltip("점프 후 발소리가 나지 않을 시간(초)입니다.")]
    public float muteDurationAfterJump = 1.0f; // ★ 추가된 부분

    // --- 내부 변수들 ---
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    private Coroutine footstepCoroutine;
    private bool isMutedByJump = false; // ★ 추가된 부분: 점프로 인해 소리가 꺼졌는지 확인

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- 1. 점프 입력 확인 ---
        // 스페이스바를 "누르는 순간"을 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 점프를 하면, 소리가 나지 않도록 타이머를 작동시킴
            StartCoroutine(MuteFootstepsAfterJump());
        }

        // --- 2. 걷기 입력 및 상태 확인 ---
        // 'A' 키 또는 'D' 키를 누르고 있고, "점프로 인해 소리가 꺼진 상태가 아닐 때"만
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isMutedByJump)
        {
            // 발소리 코루틴이 실행되고 있지 않다면, 시작
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            // 키를 누르고 있지 않거나, 점프 때문에 소리가 꺼진 상태라면,
            // 발소리 코루틴을 멈춤
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }

    // ★ 추가된 코루틴: 점프 후 일정 시간 동안 발소리를 끔
    IEnumerator MuteFootstepsAfterJump()
    {
        // 1. 소리가 나지 않도록 상태를 변경
        isMutedByJump = true;

        // 2. 인스펙터에서 설정한 시간만큼 기다림
        yield return new WaitForSeconds(muteDurationAfterJump);

        // 3. 시간이 지나면 다시 소리가 날 수 있도록 상태를 원상 복구
        isMutedByJump = false;
    }

    // 발소리를 주기적으로 재생하는 코루틴
    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepSounds != null && footstepSounds.Length > 0)
            {
                AudioClip clipToPlay = footstepSounds[currentClipIndex];

                if (clipToPlay != null)
                {
                    audioSource.PlayOneShot(clipToPlay, footstepVolume);
                }

                currentClipIndex++;
                if (currentClipIndex >= footstepSounds.Length)
                {
                    currentClipIndex = 0;
                }
            }

            yield return new WaitForSeconds(footstepInterval);
        }
    }
}
