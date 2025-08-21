using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLance_01 : MonoBehaviour
{
    [SerializeField]
    private Collider2D coll;

    [SerializeField]
    private GameObject HalfHitGroundLancePrefab;//�������蔻��̑��n�`�v���n�u
    void Start()
    {
        
    }

    void Update()
    {
        return;

        if (coll != null)
        {
            if (Collision_Manager.GetTouchingObjectWithLayer(coll, "Platform"))
            {
                Destroy(this.gameObject);
            }
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            if (HalfHitGroundLancePrefab != null)
            {
                GameObject HalfHitGroundLance = Instantiate(HalfHitGroundLancePrefab, transform.position, transform.rotation);//�����𐶐�
                HalfHitGroundLance.transform.localScale = this.transform.localScale;//�傫���������p��
            }

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
