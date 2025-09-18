using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class FaceDirectionOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1. 이 오브젝트에 있는 SpriteRenderer 컴포넌트를 찾아옵니다.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // 2. SpriteRenderer의 flipX 값을 true로 설정하여 X축으로 반전시킵니다.
        //    (이는 Inspector 창의 Flip -> X 체크박스를 체크하는 것과 동일합니다.)
        spriteRenderer.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
