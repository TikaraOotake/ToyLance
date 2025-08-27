using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ¾À °E®¸¦ À§ÇØ ÇÊ¿äÇÕ´Ï´Ù.

public class TitleManager : MonoBehaviour
{
    [Header("¼³Á¤")]
    // Inspector¿¡¼­ ¾Ö´Ï¸ŞÀÌ¼ÇÀ» Àç»ıÇÒ Animator¸¦ ¿¬°áÇÕ´Ï´Ù.
    public Animator titleAnimator;

    // Inspector¿¡¼­ ¾Ö´Ï¸ŞÀÌ¼ÇÀÌ ³¡³­ ÈÄ ³Ñ¾ûÌ¥ ¾ÀÀÇ ÀÌ¸§À» ÀÔ·ÂÇÕ´Ï´Ù.
    public string nextSceneName;

    // ¾Ö´Ï¸ŞÀÌ¼Ç TriggerÀÇ ÀÌ¸§À» Inspector¿¡¼­ ¼³Á¤ÇÕ´Ï´Ù.
    public string animationTriggerName = "Start";

    // Å° ÀÔ·ÂÀÌ °¡´ÉÇÑ »óÅÂÀÎÁEÈ®ÀÎÇÏ´Â º¯¼öÀÔ´Ï´Ù.
    private bool canPressKey = true;

    void Update()
    {
        // Å° ÀÔ·ÂÀÌ °¡´ÉÇÑ »óÅÂÀÌ°E ¾Æ¹« Å°³ª ´­·ÈÀ» ¶§
        if (canPressKey && Input.anyKeyDown)
        {
            // Å° ÀÔ·ÂÀ» ºñÈ°¼ºÈ­ÇÏ¿© Áßº¹ ½ÇÇàÀ» ¹æÁöÇÕ´Ï´Ù.
            canPressKey = false;

            // Animator°¡ ¿¬°áµÇ¾EÀÖ´Ù¸E ¼³Á¤µÈ ÀÌ¸§ÀÇ Trigger¸¦ ½ÇÇàÇÕ´Ï´Ù.
            if (titleAnimator != null)
            {
                titleAnimator.SetTrigger(animationTriggerName);
            }
        }
    }

    // ¡Ú ¾Ö´Ï¸ŞÀÌ¼Ç ÀÌº¥Æ®¿¡¼­ È£ÃâÇÒ ÇÔ¼öÀÔ´Ï´Ù.
    // ÀÌ ÇÔ¼ö´Â ¾Ö´Ï¸ŞÀÌ¼Ç Å¬¸³ÀÇ ¸¶Áö¸· ÇÁ·¹ÀÓ¿¡ Ãß°¡ÇØ¾ß ÇÕ´Ï´Ù.
    public void OnAnimationFinished()
    {
        // 0.5ÃÊ µÚ¿¡ LoadNextScene ÇÔ¼ö¸¦ ½ÇÇàÇÏµµ·Ï ¿¹¾àÇÕ´Ï´Ù.
        Invoke("LoadNextScene", 0.5f);
    }

    // ´ÙÀ½ ¾ÀÀ» ºÒ·¯¿À´Â ÇÔ¼öÀÔ´Ï´Ù.
    private void LoadNextScene()
    {
        // Inspector¿¡¼­ ¼³Á¤ÇÑ ÀÌ¸§ÀÇ ¾ÀÀ» ºÒ·¯¿É´Ï´Ù.
        SceneManager.LoadScene(nextSceneName);
    }
}
