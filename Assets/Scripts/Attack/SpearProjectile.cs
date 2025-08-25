using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

public class SpearProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float flyLife = 3f;
    [SerializeField] private float stuckLife = 15f;

    private Rigidbody2D rb;
    private bool stuck = false;
    private int dir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(int dir, float speed)
    {
        this.dir = dir;
        rb.velocity = Vector2.right * this.dir * speed;
        GetComponent<SpriteRenderer>().flipX = (this.dir < 0);
        Destroy(gameObject, flyLife);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (stuck) return;

        // ｡ﾚ｡ﾚ｡ﾚ ｿｩｱ箍｡ ｼ､ｵﾈ ｷﾎﾁﾔｴﾏｴﾙ ｡ﾚ｡ﾚ｡ﾚ
        // 1. ｺﾎｵ抦・ｴ・ﾌ ﾆﾄｱｫ ﾆｮｸｮｰﾅﾀﾎﾁ・ｸﾕﾀ・ﾈｮﾀﾎ
        if (col.CompareTag("DestructionTriggerTag"))
        {
            // ﾆｮｸｮｰﾅﾀﾇ ﾆﾄｱｫ ﾇﾔｼｦ ﾈ｣ﾃ・
            col.GetComponent<DestructionTrigger>()?.TriggerDestruction();

            // ﾃ｢ ﾀﾚｽﾅｵｵ ﾆﾄｱｫ
            Destroy(gameObject);
            return;
        }

        // --- ﾀﾌﾇﾏ ｱ簔ｸﾀﾇ ｴﾙｸ･ ﾃ豬ｹ ｷﾎﾁ・---
        if (col.gameObject.layer == LayerMask.NameToLayer("SpearPlatform"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
            return;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            StickToTarget(col.transform);
            return;
        }

        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform.position, false);
            Destroy(gameObject);
            return;
        }
    }

    void StickToTarget(Transform parent)
    {
        stuck = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(parent);
        GetComponent<Collider2D>().enabled = false;

        var platformCollider = gameObject.AddComponent<BoxCollider2D>();
        platformCollider.size = new Vector2(0.2f, 3.6f);
        platformCollider.offset = new Vector2(0, 0.0f);
        platformCollider.isTrigger = false;
        platformCollider.usedByEffector = true;

        var effector = gameObject.AddComponent<PlatformEffector2D>();
        effector.useOneWay = true;
        effector.surfaceArc = 160;
        effector.rotationalOffset = (dir > 0) ? -90 : 90;

        gameObject.layer = LayerMask.NameToLayer("SpearPlatform");
        Destroy(gameObject, stuckLife);
    }

    //槍が盾に刺さった時の反転処理
    public void FlipInShield()
    {
        Vector3 spearPos = transform.localPosition;
        spearPos.x *= -1;
        transform.localPosition = spearPos;

        Vector3 spearRot = transform.localEulerAngles;
        if (spearRot.y == 0f)
        {
            spearRot.y = 180f;
        }
        else
        {
            spearRot.y = 0f;
        }
        transform.localEulerAngles = spearRot;
    }
}
