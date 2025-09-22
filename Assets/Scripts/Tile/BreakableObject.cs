using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    Effecter effecter;
    DestroySoundPlayer soundPlayer;

    private void Start()
    {
        effecter = GetComponent<Effecter>();
        soundPlayer = GetComponent<DestroySoundPlayer>();
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

        if (soundPlayer != null) soundPlayer.PlaySound();

        //���g��j��
        Destroy(gameObject);
    }
}
