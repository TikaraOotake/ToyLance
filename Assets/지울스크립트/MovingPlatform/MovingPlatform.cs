using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("이동 지점 설정")]
    [Tooltip("플랫폼이 이동을 시작할 지점입니다.")]
    public Transform pointA;

    [Tooltip("플랫폼이 이동할 목표 지점입니다.")]
    public Transform pointB;

    [Header("플랫폼 설정")]
    [Tooltip("플랫폼의 이동 속도입니다.")]
    public float speed = 2.0f;

    // ★★★ 추가된 부분 ★★★
    [Header("플레이어 감지 설정")]
    [Tooltip("플레이어로 인식할 레이어를 선택해주세요.")]
    public LayerMask playerLayer;
    // ★★★★★★★★★★★★★

    // --- 내부 변수 ---
    private Transform currentTarget;
    private Transform currentlyCarryingPlayer = null; // 현재 태우고 있는 플레이어
    private BoxCollider2D platformCollider; // 발판의 콜라이더

    void Start()
    {
        transform.position = pointA.position;
        currentTarget = pointB;
        platformCollider = GetComponent<BoxCollider2D>(); // 발판의 BoxCollider2D를 찾아옴
    }

    void FixedUpdate()
    {
        // --- 1. 플랫폼 이동 로직 (기존과 동일) ---
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
        }

        // --- 2. 플레이어 감지 및 탑승 처리 로직 (새로운 방식) ---
        DetectAndCarryPlayer();
    }

    // ★★★ 핵심 로직: 발판 위를 확인하여 플레이어를 태우거나 내리게 함 ★★★
    void DetectAndCarryPlayer()
    {
        // 발판의 윗면 중앙에서 발판 너비만큼 위로 짧은 상자 모양의 레이(BoxCast)를 쏨
        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f); // 너비는 살짝 좁게, 높이는 짧게
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        // 만약 레이가 플레이어와 부딪혔다면,
        if (hit.collider != null)
        {
            // 아직 이 플레이어를 태우고 있지 않다면,
            if (currentlyCarryingPlayer != hit.transform)
            {
                // 플레이어를 자식으로 만들어 태웁니다.
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else // 레이에 아무것도 닿지 않았다면 (플레이어가 내렸다면),
        {
            // 이전에 태우고 있던 플레이어가 있다면,
            if (currentlyCarryingPlayer != null)
            {
                // 자식 관계를 해제하여 자유롭게 만듭니다.
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }

    // OnCollisionEnter2D와 OnCollisionExit2D는 더 이상 사용하지 않으므로 삭제합니다.
}