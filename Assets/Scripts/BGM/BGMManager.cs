using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    [Header("BGM Settings")]
    public AudioClip bgmClip;
    [Range(0f, 1f)]
    public float baseVolume = 0.8f;
    [Range(0f, 2f)]
    public float volumeMultiplier = 1.0f;
    public bool loop = true;

    [Header("BGM Zone Settings")]
    public string bgmZoneName;

    private AudioSource audioSource;
    public static BGMManager instance;
    private static string currentZoneName;
    private static AudioClip currentClip;

    void Awake()
    {
        if (currentZoneName != bgmZoneName)
        {
            if (instance != null) { Destroy(instance.gameObject); }
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentZoneName = bgmZoneName;
            currentClip = bgmClip;
            audioSource = GetComponent<AudioSource>();
            ApplyAudioSettings();
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (instance != null)
        {
            float finalVolume = instance.baseVolume * instance.volumeMultiplier;
            instance.audioSource.volume = Mathf.Clamp01(finalVolume);
        }
    }

    // ★추가: BGM을 켜고 끄는 함수
    public void ToggleBGM()
    {
        // AudioSource의 Mute(음소거) 상태를 반전시킵니다.
        audioSource.mute = !audioSource.mute;
    }

    private void ApplyAudioSettings()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        float finalVolume = baseVolume * volumeMultiplier;
        audioSource.volume = Mathf.Clamp01(finalVolume);
    }
}
