using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // �ܺ�(�÷��̾��� ����)���� �� �Լ��� ȣ���ϸ�E
    public void Break()
    {
        // (���û���) ���⿡ ��ƼŬ�̳� ���сEȿ���� ����ϴ� �ڵ带 ���� ��E�ֽ��ϴ�.
        // ��: Instantiate(destructionEffect, transform.position, Quaternion.identity);

        //�J������h�炷
        CameraManager.SetShakeCamera();

        //���g��j��
        Destroy(gameObject);
    }
}
