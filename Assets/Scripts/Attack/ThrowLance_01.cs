using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLance_01 : MonoBehaviour
{
    [SerializeField]
    private Collider2D coll;

    [SerializeField]
    private GameObject HalfHitGroundLancePrefab;//�������蔻��̑��n�`�v���n�u

    private GameObject Player;//�v���C���[

    private Effecter _effecter;

    [SerializeField]
    private SpriteBoard _spriteBoard;//�C���X�y�N�^�[��ŃA�^�b�`
    private void Awake()
    {
        Player = GameManager_01.GetPlayer();//���擾
    }
    private void Start()
    {
        _effecter = GetComponent<Effecter>();
    }

    //�����X�g�������p������
    private void HandoverLance(GameObject _new)
    {
        if (Player != null)
        {
            Player_01_Control player = Player.GetComponent<Player_01_Control>();
            if (player != null)
            {
                player.HandoverLance(this.gameObject, _new);
            }
        }
    }
    private void GenerateLancePlatform(GameObject _obj)
    {
        if (HalfHitGroundLancePrefab != null)
        {
            GameObject HalfHitGroundLance = Instantiate(HalfHitGroundLancePrefab, transform.position, Quaternion.identity);//�����𐶐�
            HalfHitGroundLance.transform.localScale = this.transform.localScale;//�傫���������p��
            HalfHitGroundLance.transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);//�p�x�������p��(Y���̂�)
            HalfHitGroundLance.transform.SetParent(_obj.transform);//�e�q�t��

            HandoverLance(HalfHitGroundLance);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�Ώە��������ł���΂��݂��폜
        if (collision.tag == "SpearPlatform")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            return;
        }

        BreakableObject breakable = collision.GetComponent<BreakableObject>();
        if (breakable != null)
        {
            if (_spriteBoard) _spriteBoard.SetShakeSprite();//����U��������
            return;
        }

        Ponballoon ponballoon = collision.GetComponent<Ponballoon>();
        Poppi poppi = collision.GetComponent<Poppi>();
        Enemy_shield shield = collision.GetComponent<Enemy_shield>();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") ||
            shield != null ||
            poppi != null)
        {
            //������̐���
            GenerateLancePlatform(collision.gameObject);

            if (_effecter) _effecter.GenerateEffect();//�G�t�F�N�g����

            Destroy(this.gameObject);
            return;
        }

		EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
		if (enemyHealth != null ||
			ponballoon != null)
		{
			if (_effecter) _effecter.GenerateEffect();//�G�t�F�N�g����
			Destroy(this.gameObject);
			return;
		}

		if (collision.CompareTag("DestructionTriggerTag"))
        {
            // Ʈ������ �ı� �Լ��� ȣÁE
            collision.GetComponent<DestructionTrigger>()?.TriggerDestruction();

            // â �ڽŵ� �ı�
            Destroy(gameObject);
            return;
        }

        // --- ���� ������ �ٸ� �浹 ����E---
        if (collision.gameObject.layer == LayerMask.NameToLayer("SpearPlatform"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            //StickToTarget(collision.transform);
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            //col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
            //Destroy(gameObject);
            //return;
        }
    }
}
