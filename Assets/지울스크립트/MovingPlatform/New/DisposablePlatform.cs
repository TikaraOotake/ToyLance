using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposablePlatform : MonoBehaviour
{
    // 외부(Spawner)에서 설정해 줄 변수들
    public Transform targetB; // 이동할 목표 지점 B
    public float speed = 3.0f; // 이동 속도
    public LayerMask playerLayer; // 플레이어 감지용 레이어

    // 내부 변수
    private Transform currentlyCarryingPlayer = null;
    private BoxCollider2D platformCollider;

    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        // 목표 지점 B가 설정되었다면 그곳으로 이동
        if (targetB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetB.position, speed * Time.deltaTime);

            // 목표 지점 B에 도달하면 스스로 파괴
            if (Vector3.Distance(transform.position, targetB.position) < 0.01f)
            {
                // 파괴되기 직전에 플레이어를 태우고 있었다면 자식 관계 해제
                if (currentlyCarryingPlayer != null)
                {
                    currentlyCarryingPlayer.SetParent(null);
                }
                Destroy(gameObject);
            }
        }

        // 플레이어 감지 및 탑승 처리
        DetectAndCarryPlayer();
    }

    // 플레이어를 감지하고 태우는 로직 (기존과 동일)
    void DetectAndCarryPlayer()
    {
        if (platformCollider == null) return;

        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        if (hit.collider != null)
        {
            if (currentlyCarryingPlayer != hit.transform)
            {
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else
        {
            if (currentlyCarryingPlayer != null)
            {
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }
}
