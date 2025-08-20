using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class ConditionalOneWayTilemap : MonoBehaviour
{
    /// <summary>
    /// 플랫폼 위·아래 분기 각도 (PlatformEffector2D.surfaceArc와 동일하게 유지)
    /// 180°라면 ±90°까지가 올라설 수 있는 구역
    /// </summary>
    [Range(0f, 180f)] public float allowedArc = 180f;

    CompositeCollider2D tilemapCollider;
    PlatformEffector2D effector;

    void Awake()
    {
        tilemapCollider = GetComponent<CompositeCollider2D>();
        effector = GetComponent<PlatformEffector2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // 플레이어가 아니면 무시
        if (!col.collider.CompareTag("Player")) return;

        // 타일맵 기준으로 플레이어가 어느 방향에 있는지 계산
        Vector2 toPlayer = (Vector2)col.collider.bounds.center - (Vector2)transform.position;
        Vector2 upDir = (Vector2)transform.up;
        float angle = Vector2.Angle(upDir, toPlayer);   // 0° = 위쪽, 90° = 옆, 180° = 아래

        bool insideArc = angle <= allowedArc * 0.5f;         // 예: 180°면 ±90°까지

        // Arc 안: 충돌 허용 / Arc 밖: 충돌 무시(통과)
        Physics2D.IgnoreCollision(col.collider, tilemapCollider, !insideArc);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // 플랫폼을 벗어났을 때 충돌 상태를 반드시 복원
        if (col.collider.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(col.collider, tilemapCollider, false);
        }
    }
}
