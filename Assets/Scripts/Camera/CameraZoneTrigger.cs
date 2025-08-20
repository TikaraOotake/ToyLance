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
            // �� ������ ������, ����� ī�޶��� �켱���� +1 �����ν� 
            if (targetCamera != null) targetCamera.Priority = nowCamera.Priority + 1;
        }
    }
}
