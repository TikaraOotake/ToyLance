using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public GameObject capsule;
    public float attackCoolTime = 0.5f;

    // ★추가: 빠른 공격의 배속을 Inspector에서 조절할 변수
    public float fastAttackSpeed = 1.5f;

    private float curTime;
    private Animator anim;
    private Player_move playerMoveScript;

    void Start()
    {
        if (capsule != null) capsule.SetActive(false);
        anim = GetComponent<Animator>();
        playerMoveScript = GetComponent<Player_move>();
    }

    void Update()
    {
        curTime += Time.deltaTime;

        if (Input.GetButtonDown("Melee") && (playerMoveScript == null || !playerMoveScript.isDownAttacking))
        {
            if (curTime >= attackCoolTime)
            {
                // ★수정: 공격 직전, 플레이어의 상태에 따라 애니메이션 속도 설정
                if (playerMoveScript != null && playerMoveScript.IsMovingOrJumping())
                {
                    // 움직이거나 점프 중일 때는 빠른 속도로 설정
                    anim.SetFloat("AttackSpeedMultiplier", fastAttackSpeed);
                }
                else
                {
                    // 가만히 서 있을 때는 기본 속도(1)로 설정
                    anim.SetFloat("AttackSpeedMultiplier", 1f);
                }

                if (anim != null) anim.SetTrigger("DoMeleeAttack");
                ActivateSideAttackHitbox();
                curTime = 0f;
            }
        }
    }

    // --- 이하 함수들은 변경 없음 ---
    void ActivateSideAttackHitbox()
    {
        if (capsule == null) return;
        bool isFlipped = GetComponent<SpriteRenderer>().flipX;
        capsule.transform.localRotation = isFlipped ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        capsule.transform.localPosition = new Vector3(isFlipped ? -0.6f : 0.6f, -0.05f, 0);
        capsule.SetActive(true);
        Invoke("DeactivateAttackHitbox", 0.2f);
    }

    public void ActivateDownAttackHitbox()
    {
        if (capsule == null) return;
        capsule.transform.localRotation = Quaternion.Euler(0, 0, -90);
        capsule.transform.localPosition = new Vector3(0, -0.6f, 0);
        capsule.SetActive(true);
    }

    public void DeactivateAttackHitbox()
    {
        if (capsule != null)
        {
            capsule.SetActive(false);
        }
    }
}
