using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // �� �� ��ȯ

/// �÷��̾ 'Door' �տ� ������ Ŭ���� ������ �̵�
[RequireComponent(typeof(Collider2D))]
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] string clearSceneName = "Clear";   // Build Settings�� ����� �̸�

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        SceneManager.LoadScene(clearSceneName);
    }
}
