using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� ��ũ��Ʈ�� ������ AudioSource�� �ڵ����� �߰��˴ϴ�.
[RequireComponent(typeof(AudioSource))]

public class AnimationSoundPlayer : MonoBehaviour
{
    [Header("���� ����")]
    [Tooltip("�ִϸ��̼� �̺�Ʈ�� ����� ����� Ŭ��")]
    public AudioClip soundClip;

    // �ڡڡ� ����� �κ� �ڡڡ�
    [Tooltip("������ ũ��(����)�� �����մϴ�. 1 �̻����� �����ϸ� �Ҹ��� �����˴ϴ�.")]
    [Range(0f, 2f)] // �ִ� ������ 1f���� 2f�� �÷� 2����� ���� ����
    public float volume = 1.0f;
    // �ڡڡڡڡڡڡڡڡڡڡڡڡ�

    [Header("��Ÿ�� ����")]
    [Tooltip("���尡 �� �� ����� ��, �ٽ� ����Ǳ���� �ʿ��� �ּ� �ð�(��)�Դϴ�.")]
    public float cooldownTime = 0.5f;

    // --- ���� ���� ---
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
                // pitch ���� ����� ���ŵǾ����ϴ�.
                audioSource.PlayOneShot(soundClip, volume);
            }
        }
    }
}
