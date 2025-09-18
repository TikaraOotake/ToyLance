using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))] // �� ��ũ��Ʈ�� ������ AudioSource�� �ڵ����� �߰��˴ϴ�.
public class FootstepPlayer : MonoBehaviour
{
    [Header("�߼Ҹ� ����� ����")]
    [Tooltip("����� �߼Ҹ� ����� ������ ������� �־��ּ���.")]
    public AudioClip[] footstepSounds;

    [Tooltip("�߼Ҹ��� ���� �ð� �����Դϴ�. (��: 0.5�ʿ� �� ��)")]
    [Range(0.1f, 2f)]
    public float footstepInterval = 0.8f;

    [Tooltip("�߼Ҹ��� ũ���Դϴ�. (0 ~ 1)")]
    [Range(0f, 1f)]
    public float footstepVolume = 1.0f;

    [Header("���� ����")]
    [Tooltip("���� �� �߼Ҹ��� ���� ���� �ð�(��)�Դϴ�.")]
    public float muteDurationAfterJump = 1.0f; // �� �߰��� �κ�

    // --- ���� ������ ---
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    private Coroutine footstepCoroutine;
    private bool isMutedByJump = false; // �� �߰��� �κ�: ������ ���� �Ҹ��� �������� Ȯ��

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- 1. ���� �Է� Ȯ�� ---
        // �����̽��ٸ� "������ ����"�� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������ �ϸ�, �Ҹ��� ���� �ʵ��� Ÿ�̸Ӹ� �۵���Ŵ
            StartCoroutine(MuteFootstepsAfterJump());
        }

        // --- 2. �ȱ� �Է� �� ���� Ȯ�� ---
        // 'A' Ű �Ǵ� 'D' Ű�� ������ �ְ�, "������ ���� �Ҹ��� ���� ���°� �ƴ� ��"��
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isMutedByJump)
        {
            // �߼Ҹ� �ڷ�ƾ�� ����ǰ� ���� �ʴٸ�, ����
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            // Ű�� ������ ���� �ʰų�, ���� ������ �Ҹ��� ���� ���¶��,
            // �߼Ҹ� �ڷ�ƾ�� ����
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }

    // �� �߰��� �ڷ�ƾ: ���� �� ���� �ð� ���� �߼Ҹ��� ��
    IEnumerator MuteFootstepsAfterJump()
    {
        // 1. �Ҹ��� ���� �ʵ��� ���¸� ����
        isMutedByJump = true;

        // 2. �ν����Ϳ��� ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(muteDurationAfterJump);

        // 3. �ð��� ������ �ٽ� �Ҹ��� �� �� �ֵ��� ���¸� ���� ����
        isMutedByJump = false;
    }

    // �߼Ҹ��� �ֱ������� ����ϴ� �ڷ�ƾ
    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepSounds != null && footstepSounds.Length > 0)
            {
                AudioClip clipToPlay = footstepSounds[currentClipIndex];

                if (clipToPlay != null)
                {
                    audioSource.PlayOneShot(clipToPlay, footstepVolume);
                }

                currentClipIndex++;
                if (currentClipIndex >= footstepSounds.Length)
                {
                    currentClipIndex = 0;
                }
            }

            yield return new WaitForSeconds(footstepInterval);
        }
    }
}
