using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public GameObject capsule;
    public float attackCoolTime = 0.5f;

    // ���߰�: ��E� ������ ����� Inspector���� ������ ����E
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
                // �ڼ���: ���� ����E �÷��̾��� ���¿� ����E�ִϸ��̼� �ӵ� ����
                if (playerMoveScript != null && playerMoveScript.IsMovingOrJumping())
                {
                    // �����̰ų� ���� ���� ���� ��E� �ӵ��� ����
                    anim.SetFloat("AttackSpeedMultiplier", fastAttackSpeed);
                }
                else
                {
                    // ����ȁE�� ���� ���� �⺻ �ӵ�(1)�� ����
                    anim.SetFloat("AttackSpeedMultiplier", 1f);
                }

                if (anim != null) anim.SetTrigger("DoMeleeAttack");
                ActivateSideAttackHitbox();
                curTime = 0f;
            }
        }
    }

    // --- ���� �Լ����� ����E���� ---
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
