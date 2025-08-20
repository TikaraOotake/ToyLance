using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // 외부(플레이어의 공격)에서 이 함수를 호출하면,
    public void Break()
    {
        // (선택사항) 여기에 파티클이나 사운드 효과를 재생하는 코드를 넣을 수 있습니다.
        // 예: Instantiate(destructionEffect, transform.position, Quaternion.identity);

        // 이 스크립트가 붙어있는 게임 오브젝트를 즉시 파괴합니다.
        Destroy(gameObject);
    }
}
