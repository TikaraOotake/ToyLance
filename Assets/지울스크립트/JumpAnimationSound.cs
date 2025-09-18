using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를 넣으면 AudioSource가 자동으로 추가됩니다.
[RequireComponent(typeof(AudioSource))]
public class JumpAnimationSound : MonoBehaviour
{
    [Header("점프 사운드 설정")]
    [Tooltip("Jump 애니메이션 재생 시 출력할 오디오 클립")]
    public AudioClip jumpSoundClip;

    [Tooltip("점프 사운드의 크기 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("쿨타임 설정")]
    [Tooltip("사운드가 한 번 재생된 후, 다시 재생되기까지 필요한 최소 시간(초)입니다.")]
    public float cooldownTime = 0.5f;

    // --- 내부 변수 ---
    private AudioSource audioSource;
    private float lastPlayTime = -1f; // 사운드가 마지막으로 재생된 시간을 기록

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // 쿨타임보다 훨씬 이전 시간으로 초기화하여 첫 이벤트 호출 시에는 무조건 소리가 나게 함
        lastPlayTime = -cooldownTime;
    }

    // ★★★ Jump 애니메이션 이벤트가 호출할 공용(public) 함수 ★★★
    public void PlayJumpSound()
    {
        // 현재 시간이 (마지막으로 소리가 난 시간 + 쿨타임)보다 클 때만 아래 코드를 실행
        if (Time.time >= lastPlayTime + cooldownTime)
        {
            // 1. 마지막 재생 시간을 현재 시간으로 기록합니다.
            lastPlayTime = Time.time;

            // 2. 사운드 클립이 있다면 재생합니다.
            if (jumpSoundClip != null)
            {
                audioSource.PlayOneShot(jumpSoundClip, volume);
            }
        }
    }
}
