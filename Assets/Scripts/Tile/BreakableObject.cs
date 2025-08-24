using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // ¿ÜºÎ(ÇÃ·¹ÀÌ¾ûÜÇ °ø°İ)¿¡¼­ ÀÌ ÇÔ¼ö¸¦ È£ÃâÇÏ¸E
    public void Break()
    {
        // (¼±ÅÃ»çÇ×) ¿©±â¿¡ ÆÄÆ¼Å¬ÀÌ³ª »ç¿ûÑEÈ¿°ú¸¦ Àç»ıÇÏ´Â ÄÚµå¸¦ ³ÖÀ» ¼EÀÖ½À´Ï´Ù.
        // ¿¹: Instantiate(destructionEffect, transform.position, Quaternion.identity);

        //ƒJƒƒ‰‚ğ—h‚ç‚·
        CameraManager.SetShakeCamera();

        //©g‚ğ”jŠü
        Destroy(gameObject);
    }
}
