using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleKnockback : MonoBehaviour
{
    [Header("�˹� ����")]
    public float knockbackForce = 5f; // �÷��̾ �о�� ���� ũ��

    // �� ��ũ��Ʈ�� ���� ������Ʈ(Circle)�� �ٸ� Collider�� ���������� �浹���� �� ȣ��˴ϴ�.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ε��� ����� �±�(Tag)�� "Player"���
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. �ε��� �÷��̾��� Rigidbody2D ������Ʈ�� �����ɴϴ�.
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // �÷��̾��� Rigidbody2D�� ������ ��쿡�� ����
            if (playerRigidbody != null)
            {
                // 2. �˹� ������ ����մϴ�. (�÷��̾� ��ġ - Circle ��ġ = Circle���� �÷��̾�� ���ϴ� ����)
                Vector2 knockbackDirection = (playerRigidbody.transform.position - transform.position).normalized;

                // 3. �÷��̾��� ���� �ӵ��� ��� 0���� ����� �˹� ȿ���� �� �� �������� �մϴ�.
                playerRigidbody.velocity = Vector2.zero;

                // 4. ���� �������� �÷��̾�� �������� ��(Impulse)�� ���մϴ�.
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
