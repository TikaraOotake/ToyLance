using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    // ★추가: 오브젝트의 최대 수명을 설정 (에디터에서 6초로 수정 가능)
    public float lifetime = 6f;

    // ★추가: 오브젝트가 생성될 때 딱 한 번 호출되는 함수
    void Start()
    {
        // lifetime(6초) 뒤에 이 오브젝트(gameObject)를 파괴하도록 예약합니다.
        Destroy(gameObject, lifetime);
    }

    // 기존의 KillZone 충돌 파괴 로직은 그대로 둡니다.
    // KillZone에 먼저 닿으면 즉시 파괴되고, 그렇지 않으면 6초 뒤에 파괴됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
}
