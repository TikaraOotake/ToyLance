using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // �ܺ�(�÷��̾��� ����)���� �� �Լ��� ȣ���ϸ�,
    public void Break()
    {
        // (���û���) ���⿡ ��ƼŬ�̳� ���� ȿ���� ����ϴ� �ڵ带 ���� �� �ֽ��ϴ�.
        // ��: Instantiate(destructionEffect, transform.position, Quaternion.identity);

        // �� ��ũ��Ʈ�� �پ��ִ� ���� ������Ʈ�� ��� �ı��մϴ�.
        Destroy(gameObject);
    }
}
