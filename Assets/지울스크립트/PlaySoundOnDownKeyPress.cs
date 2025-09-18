using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 스크립트를 넣으면 AudioSource가 자동으로 추가됩니다.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnDownKeyPress : MonoBehaviour
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


    private AudioSource audioSource;
    private float lastPlayTime = -1f; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
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
