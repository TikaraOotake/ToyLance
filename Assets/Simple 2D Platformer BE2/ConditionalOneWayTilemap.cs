using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class ConditionalOneWayTilemap : MonoBehaviour
{
    /// <summary>
    /// �÷��� �����Ʒ� �б� ���� (PlatformEffector2D.surfaceArc�� �����ϰ� ����)
    /// 180�ƶ�� ��90�Ʊ����� �ö� �� �ִ� ����
    /// </summary>
    [Range(0f, 180f)] public float allowedArc = 180f;

    CompositeCollider2D tilemapCollider;
    PlatformEffector2D effector;

    void Awake()
    {
        tilemapCollider = GetComponent<CompositeCollider2D>();
        effector = GetComponent<PlatformEffector2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // �÷��̾ �ƴϸ� ����
        if (!col.collider.CompareTag("Player")) return;

        // Ÿ�ϸ� �������� �÷��̾ ��� ���⿡ �ִ��� ���
        Vector2 toPlayer = (Vector2)col.collider.bounds.center - (Vector2)transform.position;
        Vector2 upDir = (Vector2)transform.up;
        float angle = Vector2.Angle(upDir, toPlayer);   // 0�� = ����, 90�� = ��, 180�� = �Ʒ�

        bool insideArc = angle <= allowedArc * 0.5f;         // ��: 180�Ƹ� ��90�Ʊ���

        // Arc ��: �浹 ��� / Arc ��: �浹 ����(���)
        Physics2D.IgnoreCollision(col.collider, tilemapCollider, !insideArc);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // �÷����� ����� �� �浹 ���¸� �ݵ�� ����
        if (col.collider.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(col.collider, tilemapCollider, false);
        }
    }
}
