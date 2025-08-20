using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 근접 공격만 HP를 깎고, 창이 몸에 꽂혀도 데미지 없음
[RequireComponent(typeof(Rigidbody2D))]
public class CliffMonsterHealth : MonoBehaviour
{
    [Header("HP & Damage")]
    [SerializeField] int maxHP = 100;
    [SerializeField] float knockback = 0f;
    [SerializeField] float iTime = 0.1f;

    [Header("Outline State")]
    [Tooltip("이 값이 true일 때만 창이 박힙니다.")]
    public bool isStickable = true; // 창 부착 가능 상태
    [SerializeField] float outlineThickness = 0.05f; // 외곽선 두께
    [SerializeField] float outlineResetTime = 5f; // 외곽선이 다시 생기는 시간

    int currentHP;
    bool invincible;
    Rigidbody2D rb;
    Material mat; // 쉐이더 제어용 머티리얼

    void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        // SpriteRenderer에서 현재 사용중인 머티리얼의 인스턴스를 가져옵니다.
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        UpdateOutlineState(); // 시작할 때 외곽선 상태 업데이트
    }

    /* 근접 공격(MeleeHitbox)이 호출 */
    public void TakeMelee(int dmg, Vector2 attackerPos)
    {
        // 1. 창 부착 가능 상태(외곽선 O)일 때 -> 외곽선을 벗겨내고 데미지는 없음
        if (isStickable)
        {
            isStickable = false;
            UpdateOutlineState();
            CancelInvoke(nameof(ResetOutline)); // 기존의 리셋 예약이 있다면 취소
            Invoke(nameof(ResetOutline), outlineResetTime); // 일정 시간 후 외곽선 리셋 예약
            return;
        }

        // 2. 창 부착 불가능 상태(외곽선 X)일 때 -> 정상적으로 데미지를 받음
        if (invincible) return;
        StartCoroutine(IFrame());

        currentHP -= dmg;

        float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
        rb.AddForce(new Vector2(side, 0f) * knockback, ForceMode2D.Impulse);

        if (currentHP <= 0) Destroy(gameObject);
    }

    /* 원거리 창이 몸에 박힐 때 호출 – 이 스크립트가 아닌, 창 스크립트에서 상태를 직접 확인 */
    public void AttachSpear(Transform spear)
    {
        spear.SetParent(transform);
    }

    // 외곽선 상태를 머티리얼(쉐이더)에 적용하는 함수
    void UpdateOutlineState()
    {
        if (isStickable)
        {
            mat.SetFloat("_OutlineThickness", outlineThickness);
        }
        else
        {
            mat.SetFloat("_OutlineThickness", 0f);
        }
    }

    // 외곽선 상태를 리셋하는 함수
    void ResetOutline()
    {
        isStickable = true;
        UpdateOutlineState();
    }

    IEnumerator IFrame()
    {
        invincible = true;
        yield return new WaitForSeconds(iTime);
        invincible = false;
    }
}
