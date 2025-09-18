using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    Effecter effecter;

    private void Start()
    {
        effecter = GetComponent<Effecter>();
    }
    // �ܺ�(�÷��̾��� ����)���� �� �Լ��� ȣ���ϸ�E
    public void Break()
    {
        // (���û���) ���⿡ ��ƼŬ�̳� ���сEȿ���� ����ϴ� �ڵ带 ���� ��E�ֽ��ϴ�.
        // ��: Instantiate(destructionEffect, transform.position, Quaternion.identity);

        //�J������h�炷
        CameraManager.SetShakeCamera();

        if (effecter != null)
        {
            effecter.GenerateEffect();
        }

        //���g��j��
        Destroy(gameObject);
    }
}
