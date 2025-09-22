using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundPlayer : MonoBehaviour
{
    [Header("»ç¿ûÑE¼³Á¤")]
    public AudioClip destroySoundClip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float pitch = 1.0f;
    public float playDuration = 0f;

    void OnDestroy()
    {
        //‚±‚±‚Åˆ—‚ğ‚·‚é‚ÆƒGƒ‰[‚ª‹N‚«‚é‚½‚ßˆÚ“®
    }

    public void PlaySound()
    {
        // SceneManagerHelper°¡ ¾À ÀE¯ ÁßÀÌ¶ó°E¾Ë·ÁÁÖ¸E¿©±â¼­ ÇÔ¼ö¸¦ ³¡³¿
        if (SceneManagerHelper.IsSwitchingScene)
        {
            return;
        }

        if (!Application.isPlaying || destroySoundClip == null) return;

        // ÀÓ½Ã »ç¿ûÑE¿ÀºE§Æ® »ı¼º ¹× Àç»ı
        GameObject soundPlayerObject = new GameObject("TempAudio");
        soundPlayerObject.transform.position = transform.position;

        AudioSource audioSource = soundPlayerObject.AddComponent<AudioSource>();
        audioSource.clip = destroySoundClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0;
        audioSource.Play();

        float duration = (playDuration > 0) ? playDuration : destroySoundClip.length;

        // ¾À ÀE¯ ½Ã ÀÓ½Ã ¿ÀºE§Æ®°¡ ÆÄ±«µÇ´Â °ÍÀ» ¸·±EÀ§ÇØ Ãß°¡
        DontDestroyOnLoad(soundPlayerObject);
        Destroy(soundPlayerObject, duration);
    }
}
