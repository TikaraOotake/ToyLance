using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleKnockback : MonoBehaviour
{
    [Header("넉백 설정")]
    public float knockbackForce = 5f; // 플레이어를 밀어내는 힘의 크기

    // 이 스크립트가 붙은 오브젝트(Circle)가 다른 Collider와 물리적으로 충돌했을 때 호출됩니다.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 부딪힌 대상의 태그(Tag)가 "Player"라면
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 부딪힌 플레이어의 Rigidbody2D 컴포넌트를 가져옵니다.
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // 플레이어의 Rigidbody2D가 존재할 경우에만 실행
            if (playerRigidbody != null)
            {
                // 2. 넉백 방향을 계산합니다. (플레이어 위치 - Circle 위치 = Circle에서 플레이어로 향하는 방향)
                Vector2 knockbackDirection = (playerRigidbody.transform.position - transform.position).normalized;

                // 3. 플레이어의 기존 속도를 잠시 0으로 만들어 넉백 효과가 더 잘 느껴지게 합니다.
                playerRigidbody.velocity = Vector2.zero;

                // 4. 계산된 방향으로 플레이어에게 순간적인 힘(Impulse)을 가합니다.
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
