using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // ��E�� ��E�

/// �÷��̾�̡ 'Door' �տ� ��E���EŬ����E������ �̵�
[RequireComponent(typeof(Collider2D))]
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] string clearSceneName = "Clear";   // Build Settings�� ������ �̸�

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (Application.CanStreamedLevelBeLoaded(clearSceneName))
        {
            SceneManager.LoadScene(clearSceneName);
        }
        else
        {
            Debug.LogError($"�V�[���� '{clearSceneName}' �͑��݂��܂���B");
        }
    }
}
