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

    GameObject Player;

    SpearAttack spear;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }

        Player = GameManager_01.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        //�����擾
        spear = collision.GetComponent<SpearAttack>();
        if (spear != null) 
        {
            AttackType attackType= spear.GetAttackType();
            //�˂��U���Ȃ�
            if (attackType == AttackType.Trust) 
            {
                float PlayerPosX = 0.0f;    //�v���C���[��x���W
                float EnemyPosX = 0.0f;     //�G��x���W
                float ShieldPosX = 0.0f;    //����x���W

                if (Player != null && Enemy != null)
                {
                    //���ꂼ���x���W���擾
                    PlayerPosX = Player.transform.position.x;
                    EnemyPosX = Enemy.transform.position.x;
                    ShieldPosX = transform.position.x;
                }

                //�v���C���[��x���W�Ə���x���W����
                float minX = Mathf.Min(PlayerPosX, ShieldPosX);     //�ŏ��l
                float maxX = Mathf.Max(PlayerPosX, ShieldPosX);     //�ő�l

                //�G��x���W���v���C���[�Ə��̊Ԃɂ��邩
                bool isEnemyBetween = EnemyPosX > minX && EnemyPosX < maxX;

                //�G��x���W���v���C���[�Ə��̊ԂɂȂ��ꍇ
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
    public void BreakShield()
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
