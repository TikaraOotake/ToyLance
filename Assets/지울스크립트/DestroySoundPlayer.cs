using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundPlayer : MonoBehaviour
{
    [Header("���сE����")]
    public AudioClip destroySoundClip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float pitch = 1.0f;
    public float playDuration = 0f;

    void OnDestroy()
    {
        //�����ŏ���������ƃG���[���N���邽�߈ړ�
    }

    public void PlaySound()
    {
        // SceneManagerHelper�� �� ��E� ���̶�E�˷��ָ�E���⼭ �Լ��� ����
        if (SceneManagerHelper.IsSwitchingScene)
        {
            return;
        }

        if (!Application.isPlaying || destroySoundClip == null) return;

        // �ӽ� ���сE����E�Ʈ ���� �� ���
        GameObject soundPlayerObject = new GameObject("TempAudio");
        soundPlayerObject.transform.position = transform.position;

        AudioSource audioSource = soundPlayerObject.AddComponent<AudioSource>();
        audioSource.clip = destroySoundClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0;
        audioSource.Play();

        float duration = (playDuration > 0) ? playDuration : destroySoundClip.length;

        // �� ��E� �� �ӽ� ����E�Ʈ�� �ı��Ǵ� ���� ����E���� �߰�
        DontDestroyOnLoad(soundPlayerObject);
        Destroy(soundPlayerObject, duration);
    }
}
