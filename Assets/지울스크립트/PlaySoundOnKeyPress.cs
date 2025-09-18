using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �ڡڡ� �� ������ ���� �߰� �ڡڡ�
// �� ��ũ��Ʈ�� ������ AudioSource�� �ڵ����� �߰��˴ϴ�.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnKeyPress : MonoBehaviour
{
    [Header("���� ����")]
    [Tooltip("����� ����� Ŭ���Դϴ�.")]
    public AudioClip soundToPlay;

    [Tooltip("������ ũ���Դϴ�. (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("��Ÿ�� ����")]
    [Tooltip("���尡 �ٽ� ����Ǳ���� �ʿ��� �ð�(��)�Դϴ�.")]
    public float cooldownTime = 1.0f;

    // �ڡڡ� �߰��� �κ� ���� �ڡڡ�
    [Header("�� ���� �� �Ҹ� �� ����")]
    [Tooltip("�� ���� ���۵� �� �Ʒ� ������ �ð� ���� �Ҹ��� ����ϴ�.")]
    public string sceneNameToMute;

    [Tooltip("���� ���۵� �� �Ҹ��� ���� ���� �ð�(��)�Դϴ�.")]
    public float initialMuteDuration = 15.0f;
    // �ڡڡ� �߰��� �κ� �� �ڡڡ�


    // --- ���� ���� ---
    private AudioSource audioSource;
    private float lastPlayTime = -1f;
    private bool isMutedBySceneStart = false; // �� �������� ���� �Ҹ��� �������� Ȯ��

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;

        // �ڡڡ� �߰��� �κ� �ڡڡ�
        // ���� ���� �̸��� �ν����Ϳ��� ������ �� �̸��� ������ Ȯ��
        if (SceneManager.GetActiveScene().name == sceneNameToMute)
        {
            // ���ٸ�, ������ �ð� ���� �Ҹ��� ���� �ڷ�ƾ�� ����
            StartCoroutine(InitialMuteCoroutine());
        }
    }

    // �ڡڡ� �߰��� �ڷ�ƾ �ڡڡ�
    IEnumerator InitialMuteCoroutine()
    {
        // 1. �� ���� �� �Ҹ��� ���� �ʵ��� ���¸� ����
        isMutedBySceneStart = true;

        // 2. �ν����Ϳ��� ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(initialMuteDuration);

        // 3. �ð��� ������ �ٽ� �Ҹ��� �� �� �ֵ��� ���¸� ���� ����
        isMutedBySceneStart = false;
    }

    void Update()
    {
        // �ڡڡ� ����� �κ� �ڡڡ�
        // �Է��� �ְ� "�׸���" �� �������� ���� �Ҹ��� ���� ���°� "�ƴ� ��"��
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("joystick button 5")) && !isMutedBySceneStart)
        {
            // ��Ÿ�� Ȯ�� ����
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
