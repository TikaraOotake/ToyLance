using System.Collections.Generic;
using UnityEngine;

public class LiftParent : MonoBehaviour
{
    // 接触中のオブジェクト一覧
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
            //Debug.Log("プレイヤー接触");
            //other.transform.SetParent(transform);
            if (!contactObjects.Contains(other.gameObject))
            {
                contactObjects.Add(other.gameObject);
                Debug.Log($"接触追加: {other.gameObject.name}");
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
                platformVelocity.y = 0.0f;//Y軸は無視
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
                Debug.Log($"接触削除: {other.gameObject.name}");
            }

            other.transform.SetParent(null);
        }

    }



    // 足場が消えるタイミングで呼び出す関数例
    public void DisablePlatform()
    {
        // まず親子付け解除
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.SetParent(null);
            }
        }

        // 足場の無効化（非表示＆当たり判定OFFなど）
        gameObject.SetActive(false);
    }
}
