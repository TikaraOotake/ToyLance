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
            //エフェクトを生成
            Effecter effecter = GetComponent<Effecter>();
            if (effecter != null) 
            {
                effecter.GenerateEffect();
            }

            //自身を破棄
            Destroy(this.gameObject);
        }
    }
}
