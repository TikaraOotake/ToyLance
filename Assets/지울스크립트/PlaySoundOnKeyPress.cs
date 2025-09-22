using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ¡Ú¡Ú¡Ú ¾À °E®¸¦ À§ÇØ Ãß°¡ ¡Ú¡Ú¡Ú
// ÀÌ ½ºÅ©¸³Æ®¸¦ ³ÖÀ¸¸EAudioSource°¡ ÀÚµ¿À¸·Î Ãß°¡µË´Ï´Ù.
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnKeyPress : MonoBehaviour
{
    [Header("»ç¿ûÑE¼³Á¤")]
    [Tooltip("Àç»ıÇÒ ¿Àµğ¿À Å¬¸³ÀÔ´Ï´Ù.")]
    public AudioClip soundToPlay;

    [Tooltip("»ç¿ûÑåÀÇ Å©±âÀÔ´Ï´Ù. (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Header("ÄğÅ¸ÀÓ ¼³Á¤")]
    [Tooltip("»ç¿ûÑå°¡ ´Ù½Ã Àç»ıµÇ±â±ûİEÇÊ¿äÇÑ ½Ã°£(ÃÊ)ÀÔ´Ï´Ù.")]
    public float cooldownTime = 1.0f;

    // ¡Ú¡Ú¡Ú Ãß°¡µÈ ºÎºĞ ½ÃÀÛ ¡Ú¡Ú¡Ú
    [Header("¾À ½ÃÀÛ ½Ã ¼Ò¸® ²E¼³Á¤")]
    [Tooltip("ÀÌ ¾ÀÀÌ ½ÃÀÛµÉ ¶§ ¾Æ·¡ ¼³Á¤µÈ ½Ã°£ µ¿¾È ¼Ò¸®¸¦ ²û±é´Ï´Ù.")]
    public string sceneNameToMute;

    [Tooltip("¾ÀÀÌ ½ÃÀÛµÈ ÈÄ ¼Ò¸®°¡ ³ªÁE¾ÊÀ» ½Ã°£(ÃÊ)ÀÔ´Ï´Ù.")]
    public float initialMuteDuration = 15.0f;
    // ¡Ú¡Ú¡Ú Ãß°¡µÈ ºÎºĞ ³¡ ¡Ú¡Ú¡Ú


    // --- ³»ºÎ º¯¼E---
    private AudioSource audioSource;
    private float lastPlayTime = -1f;
    private bool isMutedBySceneStart = false; // ¾À ½ÃÀÛÀ¸·Î ÀÎÇØ ¼Ò¸®°¡ ²¨Á³´ÂÁEÈ®ÀÎ

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastPlayTime = -cooldownTime;

        // ¡Ú¡Ú¡Ú Ãß°¡µÈ ºÎºĞ ¡Ú¡Ú¡Ú
        // ÇöÀE¾ÀÀÇ ÀÌ¸§ÀÌ ÀÎ½ºÆåÅÍ¿¡¼­ ÁöÁ¤ÇÑ ¾À ÀÌ¸§°E°°ÀºÁEÈ®ÀÎ
        if (SceneManager.GetActiveScene().name == sceneNameToMute)
        {
            // °°´Ù¸E ¼³Á¤µÈ ½Ã°£ µ¿¾È ¼Ò¸®¸¦ ²ô´Â ÄÚ·çÆ¾À» ½ÃÀÛ
            StartCoroutine(InitialMuteCoroutine());
        }
    }

    // ¡Ú¡Ú¡Ú Ãß°¡µÈ ÄÚ·çÆ¾ ¡Ú¡Ú¡Ú
    IEnumerator InitialMuteCoroutine()
    {
        // 1. ¾À ½ÃÀÛ ½Ã ¼Ò¸®°¡ ³ªÁE¾Êµµ·Ï »óÅÂ¸¦ º¯°E
        isMutedBySceneStart = true;

        // 2. ÀÎ½ºÆåÅÍ¿¡¼­ ¼³Á¤ÇÑ ½Ã°£¸¸Å­ ±â´Ù¸²
        yield return new WaitForSeconds(initialMuteDuration);

        // 3. ½Ã°£ÀÌ Áö³ª¸E´Ù½Ã ¼Ò¸®°¡ ³¯ ¼EÀÖµµ·Ï »óÅÂ¸¦ ¿ø»Eº¹±¸
        isMutedBySceneStart = false;
    }

    void Update()
    {
        // ¡Ú¡Ú¡Ú º¯°æµÈ ºÎºĞ ¡Ú¡Ú¡Ú
        // ÀÔ·ÂÀÌ ÀÖ°E"±×¸®°E ¾À ½ÃÀÛÀ¸·Î ÀÎÇØ ¼Ò¸®°¡ ²¨ÁE»óÅÂ°¡ "¾Æ´Ò ¶§"¸¸
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && !isMutedBySceneStart)
        {
            // ÄğÅ¸ÀÓ È®ÀÎ ·ÎÁE
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
