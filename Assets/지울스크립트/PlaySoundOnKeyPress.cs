using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �ڡڡ� �� ��E��� ���� �߰� �ڡڡ�
// �� ��ũ��Ʈ�� ������EAudioSource�� �ڵ����� �߰��˴ϴ�.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnKeyPress : MonoBehaviour
{
    [Header("���сE����")]
    [Tooltip("����� ����� Ŭ���Դϴ�.")]
    public AudioClip soundToPlay;

    [Tooltip("������� ũ���Դϴ�. (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("��Ÿ�� ����")]
    [Tooltip("����尡 �ٽ� ����Ǳ��݁E�ʿ��� �ð�(��)�Դϴ�.")]
    public float cooldownTime = 1.0f;

    // �ڡڡ� �߰��� �κ� ���� �ڡڡ�
    [Header("�� ���� �� �Ҹ� ��E����")]
    [Tooltip("�� ���� ���۵� �� �Ʒ� ������ �ð� ���� �Ҹ��� ����ϴ�.")]
    public string sceneNameToMute;

    [Tooltip("���� ���۵� �� �Ҹ��� ����E���� �ð�(��)�Դϴ�.")]
    public float initialMuteDuration = 15.0f;
    // �ڡڡ� �߰��� �κ� �� �ڡڡ�


    // --- ���� ����E---
    private AudioSource audioSource;
    private float lastPlayTime = -1f;
    private bool isMutedBySceneStart = false; // �� �������� ���� �Ҹ��� ��������EȮ��

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;

        // �ڡڡ� �߰��� �κ� �ڡڡ�
        // ����E���� �̸��� �ν����Ϳ��� ������ �� �̸���E������EȮ��
        if (SceneManager.GetActiveScene().name == sceneNameToMute)
        {
            // ���ٸ�E ������ �ð� ���� �Ҹ��� ���� �ڷ�ƾ�� ����
            StartCoroutine(InitialMuteCoroutine());
        }
    }

    // �ڡڡ� �߰��� �ڷ�ƾ �ڡڡ�
    IEnumerator InitialMuteCoroutine()
    {
        // 1. �� ���� �� �Ҹ��� ����E�ʵ��� ���¸� ����E
        isMutedBySceneStart = true;

        // 2. �ν����Ϳ��� ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(initialMuteDuration);

        // 3. �ð��� ������E�ٽ� �Ҹ��� �� ��E�ֵ��� ���¸� ����E����
        isMutedBySceneStart = false;
    }

    void Update()
    {
        // �ڡڡ� ����� �κ� �ڡڡ�
        // �Է��� �ְ�E"�׸���E �� �������� ���� �Ҹ��� ����E���°� "�ƴ� ��"��
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && !isMutedBySceneStart)
        {
            // ��Ÿ�� Ȯ�� ����E
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
