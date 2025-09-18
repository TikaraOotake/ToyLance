using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledPlatform : MonoBehaviour
{
    [Header("연동할 스위치 설정")]
    [Tooltip("이 발판을 제어할 SwitchButton 스크립트를 가진 오브젝트를 연결하세요.")]
    public SwitchButton controlSwitch;

    [Header("이동 지점 및 속도 설정")]
    [Tooltip("스위치가 ON일 때 이동할 목표 지점 오브젝트를 연결하세요.")]
    public Transform targetPositionB;

    [Tooltip("발판의 이동 속도입니다.")]
    public float speed = 3.0f;

    [Header("플레이어 감지 설정")]
    [Tooltip("플레이어로 인식할 레이어를 선택해주세요.")]

    // --- 내부 변수 ---
    private Vector3 originalPositionA; // 발판의 원래 시작 위치 (A 지점)
    private Vector3 currentTargetPosition; // 현재 이동해야 할 목표 위치
    private Transform currentTarget;
    private Transform currentlyCarryingPlayer = null; // 현재 태우고 있는 플레이어
    private BoxCollider2D platformCollider; // 발판의 콜라이더
    public LayerMask playerLayer;
    void Start()
    {
        // 게임이 시작될 때, 현재 위치를 'A 지점'으로 저장합니다.
        originalPositionA = transform.position;
        // 초기 목표 위치도 A 지점으로 설정합니다.
        currentTargetPosition = originalPositionA;
        platformCollider = GetComponent<BoxCollider2D>(); // 발판의 BoxCollider2D를 찾아옴
    }

    void Update()
    {
        // 제어할 스위치가 연결되어 있는지 먼저 확인합니다.
        if (controlSwitch != null && targetPositionB != null)
        {
            // controlSwitch의 GetSwitchFlag() 함수를 호출하여 현재 스위치 상태를 가져옵니다.
            if (controlSwitch.GetSwitchFlag())
            {
                // 스위치가 ON 상태이면, 목표 위치를 B 지점으로 설정합니다.
                currentTargetPosition = targetPositionB.position;
            }
            else
            {
                // 스위치가 OFF 상태이면, 목표 위치를 원래 위치인 A 지점으로 설정합니다.
                currentTargetPosition = originalPositionA;
            }

            // 현재 위치에서 목표 위치(currentTargetPosition)까지 정해진 속도로 이동시킵니다.
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
        }
        DetectAndCarryPlayer();
    }

    // ★★★ 추가된 부분 시작 ★★★

    // 오브젝트가 플랫폼에 처음 닿았을 때 호출되는 함수
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
}
