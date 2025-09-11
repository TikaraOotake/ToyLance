using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLance_01 : MonoBehaviour
{
    [SerializeField]
    private Collider2D coll;

    [SerializeField]
    private GameObject HalfHitGroundLancePrefab;//半当たり判定の槍地形プレハブ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //対象物が槍床であればお互い削除
        if (collision.tag == "SpearPlatform")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            return;
        }

        Enemy_shield shield = collision.GetComponent<Enemy_shield>();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform")||
            shield != null)
        {
            //槍足場の生成
            if (HalfHitGroundLancePrefab != null)
            {
                GameObject HalfHitGroundLance = Instantiate(HalfHitGroundLancePrefab, transform.position, Quaternion.identity);//槍床を生成
                HalfHitGroundLance.transform.localScale = this.transform.localScale;//大きさを引き継ぐ
                HalfHitGroundLance.transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);//角度を引き継ぐ(Y軸のみ)
                HalfHitGroundLance.transform.SetParent(collision.transform);//親子付け
            }

            Destroy(this.gameObject);
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            return;
        }

        EnemyHealth enemyHealth= collision.GetComponent<EnemyHealth>();
        if(enemyHealth)
        {
            Destroy(this.gameObject);
            return;
        }

        if (collision.CompareTag("DestructionTriggerTag"))
        {
            // ﾆｮｸｮｰﾅﾀﾇ ﾆﾄｱｫ ﾇﾔｼｦ ﾈ｣ﾃ・
            collision.GetComponent<DestructionTrigger>()?.TriggerDestruction();

            // ﾃ｢ ﾀﾚｽﾅｵｵ ﾆﾄｱｫ
            Destroy(gameObject);
            return;
        }

        // --- ﾀﾌﾇﾏ ｱ簔ｸﾀﾇ ｴﾙｸ･ ﾃ豬ｹ ｷﾎﾁ・---
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
