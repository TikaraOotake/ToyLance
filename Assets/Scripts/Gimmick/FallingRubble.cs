using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRubble : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //�G�t�F�N�g�𐶐�
            Effecter effecter = GetComponent<Effecter>();
            if (effecter != null) 
            {
                effecter.GenerateEffect();
            }

            //���g��j��
            Destroy(this.gameObject);
        }
    }
}
