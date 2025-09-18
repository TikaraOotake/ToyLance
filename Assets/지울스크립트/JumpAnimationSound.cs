using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ��ũ��Ʈ�� ������ AudioSource�� �ڵ����� �߰��˴ϴ�.
[RequireComponent(typeof(AudioSource))]
public class JumpAnimationSound : MonoBehaviour
{
    [Header("���� ���� ����")]
    [Tooltip("Jump �ִϸ��̼� ��� �� ����� ����� Ŭ��")]
    public AudioClip jumpSoundClip;

    [Tooltip("���� ������ ũ�� (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("��Ÿ�� ����")]
    [Tooltip("���尡 �� �� ����� ��, �ٽ� ����Ǳ���� �ʿ��� �ּ� �ð�(��)�Դϴ�.")]
    public float cooldownTime = 0.5f;

    // --- ���� ���� ---
    private AudioSource audioSource;
    private float lastPlayTime = -1f; // ���尡 ���������� ����� �ð��� ���

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // ��Ÿ�Ӻ��� �ξ� ���� �ð����� �ʱ�ȭ�Ͽ� ù �̺�Ʈ ȣ�� �ÿ��� ������ �Ҹ��� ���� ��
        lastPlayTime = -cooldownTime;
    }

    // �ڡڡ� Jump �ִϸ��̼� �̺�Ʈ�� ȣ���� ����(public) �Լ� �ڡڡ�
    public void PlayJumpSound()
    {
        // ���� �ð��� (���������� �Ҹ��� �� �ð� + ��Ÿ��)���� Ŭ ���� �Ʒ� �ڵ带 ����
        if (Time.time >= lastPlayTime + cooldownTime)
        {
            // 1. ������ ��� �ð��� ���� �ð����� ����մϴ�.
            lastPlayTime = Time.time;

            // 2. ���� Ŭ���� �ִٸ� ����մϴ�.
            if (jumpSoundClip != null)
            {
                audioSource.PlayOneShot(jumpSoundClip, volume);
            }
        }
    }
}
