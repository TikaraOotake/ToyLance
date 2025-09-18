using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundPlayer : MonoBehaviour
{
    [Header("사운드 설정")]
    public AudioClip destroySoundClip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float pitch = 1.0f;
    public float playDuration = 0f;

    void OnDestroy()
    {
        // SceneManagerHelper가 씬 전환 중이라고 알려주면 여기서 함수를 끝냄
        if (SceneManagerHelper.IsSwitchingScene)
        {
            return;
        }

        if (!Application.isPlaying || destroySoundClip == null) return;

        // 임시 사운드 오브젝트 생성 및 재생
        GameObject soundPlayerObject = new GameObject("TempAudio");
        soundPlayerObject.transform.position = transform.position;

        AudioSource audioSource = soundPlayerObject.AddComponent<AudioSource>();
        audioSource.clip = destroySoundClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0;
        audioSource.Play();

        float duration = (playDuration > 0) ? playDuration : destroySoundClip.length;

        // 씬 전환 시 임시 오브젝트가 파괴되는 것을 막기 위해 추가
        DontDestroyOnLoad(soundPlayerObject);
        Destroy(soundPlayerObject, duration);
    }
}
