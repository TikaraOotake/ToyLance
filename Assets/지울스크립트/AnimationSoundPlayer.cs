using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 스크립트를 넣으면 AudioSource가 자동으로 추가됩니다.
[RequireComponent(typeof(AudioSource))]

public class AnimationSoundPlayer : MonoBehaviour
{
    [Header("사운드 설정")]
    [Tooltip("애니메이션 이벤트로 재생할 오디오 클립")]
    public AudioClip soundClip;

    // ★★★ 변경된 부분 ★★★
    [Tooltip("사운드의 크기(볼륨)를 조절합니다. 1 이상으로 설정하면 소리가 증폭됩니다.")]
    [Range(0f, 2f)] // 최대 범위를 1f에서 2f로 늘려 2배까지 증폭 가능
    public float volume = 1.0f;
    // ★★★★★★★★★★★★★

    [Header("쿨타임 설정")]
    [Tooltip("사운드가 한 번 재생된 후, 다시 재생되기까지 필요한 최소 시간(초)입니다.")]
    public float cooldownTime = 0.5f;

    // --- 내부 변수 ---
    private AudioSource audioSource;
    private float lastPlayTime = -1f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;
    }

    public void PlaySound()
    {
        if (Time.time >= lastPlayTime + cooldownTime)
        {
            lastPlayTime = Time.time;

            if (soundClip != null)
            {
                // pitch 조절 기능은 제거되었습니다.
                audioSource.PlayOneShot(soundClip, volume);
            }
        }
    }
}
