using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    // ���߰�: ������Ʈ�� �ִ� ������ ���� (�����Ϳ��� 6�ʷ� ���� ����)
    public float lifetime = 6f;

    // ���߰�: ������Ʈ�� ������ �� �� �� �� ȣ��Ǵ� �Լ�
    void Start()
    {
        // lifetime(6��) �ڿ� �� ������Ʈ(gameObject)�� �ı��ϵ��� �����մϴ�.
        Destroy(gameObject, lifetime);
    }

    // ������ KillZone �浹 �ı� ������ �״�� �Ӵϴ�.
    // KillZone�� ���� ������ ��� �ı��ǰ�, �׷��� ������ 6�� �ڿ� �ı��˴ϴ�.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
}
