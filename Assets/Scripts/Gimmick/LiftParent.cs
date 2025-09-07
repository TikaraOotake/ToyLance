using System.Collections.Generic;
using UnityEngine;

public class LiftParent : MonoBehaviour
{
    // �ڐG���̃I�u�W�F�N�g�ꗗ
    [SerializeField]
    private List<GameObject> contactObjects = new List<GameObject>();
    private void Update()
    {
        for (int i = 0; i < contactObjects.Count; ++i)
        {
            GameObject obj= contactObjects[i];
            Rigidbody2D playerRb = obj.gameObject.GetComponent<Rigidbody2D>();
            Rigidbody2D platformRb = GetComponent<Rigidbody2D>();

            if (playerRb != null && platformRb != null)
            {
                Vector2 platformVelocity = platformRb.velocity;
                Vector2 newPosition = playerRb.position + platformVelocity * Time.fixedDeltaTime;

                //playerRb.MovePosition(newPosition);
                //playerRb.transform.position = newPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("�v���C���[�ڐG");
            //other.transform.SetParent(transform);
            if (!contactObjects.Contains(other.gameObject))
            {
                contactObjects.Add(other.gameObject);
                Debug.Log($"�ڐG�ǉ�: {other.gameObject.name}");
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            Rigidbody2D platformRb = GetComponent<Rigidbody2D>();

            if (playerRb != null && platformRb != null)
            {
                Vector2 platformVelocity = platformRb.velocity;
                platformVelocity.y = 0.0f;//Y���͖���
                Vector2 newPosition = playerRb.position + platformVelocity * Time.fixedDeltaTime;

                //playerRb.MovePosition(newPosition);
                playerRb.transform.position = newPosition;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (contactObjects.Contains(other.gameObject))
            {
                contactObjects.Remove(other.gameObject);
                Debug.Log($"�ڐG�폜: {other.gameObject.name}");
            }

            other.transform.SetParent(null);
        }

    }



    // ���ꂪ������^�C�~���O�ŌĂяo���֐���
    public void DisablePlatform()
    {
        // �܂��e�q�t������
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.SetParent(null);
            }
        }

        // ����̖������i��\���������蔻��OFF�Ȃǁj
        gameObject.SetActive(false);
    }
}
