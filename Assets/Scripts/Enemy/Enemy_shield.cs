using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;

public class Enemy_shield : MonoBehaviour
{
    private float initialShieldX;
    private bool isFacingRight = false;

    private Enemy_move enemyMove;

    private bool isBroken = false;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        initialShieldX = transform.localPosition.x;

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }
    }

    //���̔��]����
    public void UpdateShieldPosition(bool FacingRight)
    {
        if (FacingRight == isFacingRight)
        {
            return;
        }

        isFacingRight = FacingRight;

        float direction;

        if (FacingRight)
        {
            direction = 1f;
        }
        else
        {
            direction = -11f;
        }

        Vector3 pos = transform.localPosition;
        pos.x = initialShieldX * direction;
        transform.localPosition = pos;
        foreach (Transform child in transform)
        {
            var spear = child.GetComponent<SpearProjectile>();
            if (spear != null)
            {
                //�������Ɏh���������̔��]����
                spear.FlipInShield();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        SpearAttack spear = collision.GetComponent<SpearAttack>();
        if (spear != null) 
        {
            AttackType attackType= spear.GetAttackType();
            if (attackType == AttackType.Trust) 
            {
                BreakShield();
            }
        }

        if (collision.CompareTag("PlayerMeleeAttack")) 
        {
            var meleeHitbox = collision.GetComponent<MeleeHitbox>();
            if (meleeHitbox != null) 
            {
                var player = meleeHitbox.GetComponentInParent<Player_move>();
                if (player != null && !player.isDownAttacking)
                {
                    BreakShield();
                }
            }
        }

        if (collision.GetComponent<SpearProjectile>() != null)
        {
            return;
        }
    }

    //���̔j�󏈗�
    private void BreakShield()
    {
        if (isBroken)
        {
            return;
        }

        isBroken = true;

        if (enemyMove != null)
        {
            enemyMove.PauseMovement(1f);
        }

        gameObject.SetActive(false);

        Invoke(nameof(RestoreShield), 6f);
    }

    //���̍Đ�����
    private void RestoreShield()
    {
        isBroken = false;

        gameObject.SetActive(true);
    }

    //���̃A�j���[�V�����̍Đ�����
    public void SetWalkSpeed(int speed)
    {
        if (anim != null)
        {
            anim.SetInteger("WalkSpeed", speed);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
