using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;

public class Enemy_shield : MonoBehaviour
{
    private Enemy_move enemyMove;

    public bool isBroken = false;   //���Ă��邩�̃t���O

    Animator anim;

    [SerializeField] private GameObject Enemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        //�����擾
        SpearAttack spear = collision.GetComponent<SpearAttack>();
        if (spear != null) 
        {
            AttackType attackType= spear.GetAttackType();
            //�˂��U���Ȃ�
            if (attackType == AttackType.Trust) 
            {
                GameObject Player = GameManager_01.GetPlayer();
                float PlayerPosX = 0.0f;
                float EnemyPosX = 0.0f;
                float ShieldPosX = 0.0f;

                if (Player != null && Enemy != null)
                {
                    PlayerPosX = Player.transform.position.x;
                    EnemyPosX = Enemy.transform.position.x;
                    ShieldPosX = transform.position.x;
                }

                float minX = Mathf.Min(PlayerPosX, ShieldPosX);
                float maxX = Mathf.Max(PlayerPosX, ShieldPosX);

                bool isEnemyBetween = EnemyPosX > minX && EnemyPosX < maxX;

                //�v���C���[�ƓG�̊Ԃɏ�����������
                if (!isEnemyBetween)
                {
                    //���̔j�󏈗�
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

        //���Ă���
        isBroken = true;

        var enemyHealth = GetComponentInParent<EnemyHealth_ToySoldier>();
        if (enemyHealth != null)
        {
            enemyHealth.SetShieldJustBrokeFlag();
        }

        if (enemyMove != null)
        {
            //��~����
            enemyMove.PauseMovement(1f);
        }

        //��\��
        //gameObject.SetActive(false);
        StartCoroutine(DelaySetActive(false));

        //6�b��ɏ����Đ�
        Invoke(nameof(RestoreShield), 6f);
    }

    IEnumerator DelaySetActive(bool _flag)
    {
        yield return null;
        yield return null;

        //��F��ɔ�L����
        gameObject.SetActive(_flag);
    }

    //���̍Đ�����
    private void RestoreShield()
    {
        //���Ă��Ȃ�
        isBroken = false;

        //�\��
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
