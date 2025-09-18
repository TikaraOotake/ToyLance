using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class FaceDirectionOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1. �� ������Ʈ�� �ִ� SpriteRenderer ������Ʈ�� ã�ƿɴϴ�.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // 2. SpriteRenderer�� flipX ���� true�� �����Ͽ� X������ ������ŵ�ϴ�.
        //    (�̴� Inspector â�� Flip -> X üũ�ڽ��� üũ�ϴ� �Ͱ� �����մϴ�.)
        spriteRenderer.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
