using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] 
    private GameObject Shield;              //��

    private Enemy_shield shieldScript;

    private bool shieldJustBroke = false;   //�������Ă��邩�̃t���O

    private void Start()
    {
        shieldScript = GetComponentInChildren<Enemy_shield>();
    }

    public override void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        if (shieldJustBroke)
        {
            return;
        }

        GameObject Player = GameManager_01.GetPlayer();
        float PlayerPosX = 0.0f;
        float EnemyPosX = 0.0f;
        float ShieldPosX = 0.0f;

        if (Player != null && Shield != null && !shieldScript.isBroken) 
        {
            PlayerPosX = Player.transform.position.x;
            EnemyPosX = transform.position.x;
            ShieldPosX = Shield.transform.position.x;
        }

        float minX = Mathf.Min(PlayerPosX, EnemyPosX);
        float maxX = Mathf.Max(PlayerPosX, EnemyPosX);

        bool isShieldBetween = ShieldPosX > minX && ShieldPosX < maxX;

        //�v���C���[�ƓG�̊Ԃɏ����Ȃ�������
        if (!isShieldBetween)
        {
            //�_���[�W����
            if (invincible) return;
            StartCoroutine(IFrame());

            currentHP -= dmg;
            StartCoroutine(HitFlash());            // ���� �� ������E����E
            /* �� �˹�E���θ� ��ô/������ ����E���� �� */
            if (doKnockback)
            {
                float side = (transform.position.x < attackerPos.x) ? -1f : 1f;
                Vector2 dir = new Vector2(side, 0f);       // ��ƁE�˹�E

                rb.velocity = Vector2.zero;
                rb.AddForce(dir * knockback, ForceMode2D.Impulse);
            }

            if (currentHP <= 0) Die();
        }
    }

    public void SetShieldJustBrokeFlag()
    {
        shieldJustBroke = true;
        StartCoroutine(ResetShieldBreakFlag());
    }

    private IEnumerator ResetShieldBreakFlag()
    {
        // 1�t���[�������҂��ĉ���
        yield return null;
        shieldJustBroke = false;
    }
}
