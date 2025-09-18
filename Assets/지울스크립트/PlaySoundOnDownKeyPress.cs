using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� ��ũ��Ʈ�� ������ AudioSource�� �ڵ����� �߰��˴ϴ�.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnDownKeyPress : MonoBehaviour
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
