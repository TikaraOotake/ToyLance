using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyHealth_ToySoldier : EnemyHealth
{
    [SerializeField] private GameObject Shield;
    public override void TakeDamage(int dmg, Vector2 attackerPos, bool doKnockback = true)
    {
        bool shieldAndPlayerExist = false;
        GameObject Player = GameManager_01.GetPlayer();
        float PlayerPosX = 0.0f;
        float EnemyPosX = 0.0f;
        float ShieldPosX = 0.0f;

        if (Player != null && Shield != null)
        {
            PlayerPosX = Player.transform.position.x;
            EnemyPosX = transform.position.x;
            ShieldPosX = Shield.transform.position.x;

            shieldAndPlayerExist = true;
        }

        bool isShieldBetween = false;

        if (shieldAndPlayerExist)
        {
            //���ƃv���C���[��������

            float minX = Mathf.Min(PlayerPosX, EnemyPosX);
            float maxX = Mathf.Max(PlayerPosX, EnemyPosX);

            isShieldBetween = ShieldPosX > minX && ShieldPosX < maxX;
        }
        
        if(!isShieldBetween)
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
}
