using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Enemy_c01 전용 ? 절벽(Platform 없음) 감지 후 방향 전환
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CliffEnemyMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed = 1.5f;

    [Header("Ground-sensor (child)")]
    [SerializeField] Transform groundSensor;   // 드래그
    [SerializeField] float sensorRadius = 0.06f;
    [SerializeField] LayerMask groundMask;     // Platform ✓

    Rigidbody2D rb;
    SpriteRenderer sr;

    /* 고정 오프셋(절댓값) */
    float offX;     // 0.7
    float offY;     // -1.0
    int dir = 1;  // 1=→ , -1=←
    bool wasGround = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        /* GroundSensor 의 초기 위치를 ‘절댓값’ 으로 기억 */
        offX = Mathf.Abs(groundSensor.localPosition.x);   // ★양수
        offY = groundSensor.localPosition.y;              // ★음수( -1.0 )

        groundSensor.localScale = Vector3.one;            // 부모 flipX 영향 無
    }

    void FixedUpdate()
    {
        /* ① 이동 */
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        /* ② 센서 위치 : +offX×dir, offY */
        groundSensor.localPosition = new Vector3(offX * dir, offY, 0);

        /* ③ 절벽 검사 */
        bool onGround = Physics2D.OverlapCircle(groundSensor.position,
                                                sensorRadius,
                                                groundMask);

        Debug.DrawLine(groundSensor.position,
                       groundSensor.position + Vector3.down * .08f,
                       onGround ? Color.green : Color.red);

        /* ④ 방향 전환 */
        if (!onGround && wasGround)
        {
            dir *= -1;
            sr.flipX = dir < 0;                    // 몸체는 그대로, 얼굴만 Flip
        }
        wasGround = onGround;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundSensor == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundSensor.position, sensorRadius);
    }
#endif
}
