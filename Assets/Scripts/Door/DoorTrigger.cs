using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // ← 씬 전환

/// 플레이어가 'Door' 앞에 닿으면 클리어 씬으로 이동
[RequireComponent(typeof(Collider2D))]
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] string clearSceneName = "Clear";   // Build Settings에 등록한 이름

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        SceneManager.LoadScene(clearSceneName);
    }
}
