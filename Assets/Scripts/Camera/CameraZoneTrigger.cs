using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera nowCamera;
    public CinemachineVirtualCamera targetCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Enter {gameObject.name}");
            // 이 구역에 들어오면, 연결된 카메라의 우선순위 +1 함으로써 
            if (targetCamera != null) targetCamera.Priority = nowCamera.Priority + 1;
        }
    }
}
