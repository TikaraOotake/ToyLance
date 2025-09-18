using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposablePlatform : MonoBehaviour
{
    // �ܺ�(Spawner)���� ������ �� ������
    public Transform targetB; // �̵��� ��ǥ ���� B
    public float speed = 3.0f; // �̵� �ӵ�
    public LayerMask playerLayer; // �÷��̾� ������ ���̾�

    // ���� ����
    private Transform currentlyCarryingPlayer = null;
    private BoxCollider2D platformCollider;

    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        // ��ǥ ���� B�� �����Ǿ��ٸ� �װ����� �̵�
        if (targetB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetB.position, speed * Time.deltaTime);

            // ��ǥ ���� B�� �����ϸ� ������ �ı�
            if (Vector3.Distance(transform.position, targetB.position) < 0.01f)
            {
                // �ı��Ǳ� ������ �÷��̾ �¿�� �־��ٸ� �ڽ� ���� ����
                if (currentlyCarryingPlayer != null)
                {
                    currentlyCarryingPlayer.SetParent(null);
                }
                Destroy(gameObject);
            }
        }

        // �÷��̾� ���� �� ž�� ó��
        DetectAndCarryPlayer();
    }

    // �÷��̾ �����ϰ� �¿�� ���� (������ ����)
    void DetectAndCarryPlayer()
    {
        if (platformCollider == null) return;

        Vector2 boxCastOrigin = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(platformCollider.bounds.size.x * 0.9f, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 0.1f, playerLayer);

        if (hit.collider != null)
        {
            if (currentlyCarryingPlayer != hit.transform)
            {
                hit.transform.SetParent(this.transform);
                currentlyCarryingPlayer = hit.transform;
            }
        }
        else
        {
            if (currentlyCarryingPlayer != null)
            {
                currentlyCarryingPlayer.SetParent(null);
                currentlyCarryingPlayer = null;
            }
        }
    }
}
