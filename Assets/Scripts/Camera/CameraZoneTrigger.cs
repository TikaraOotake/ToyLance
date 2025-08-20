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
            // ÀÌ ±¸¿ª¿¡ µé¾ûÛÀ¸E ¿¬°áµÈ Ä«¸Ş¶óÀÇ ¿E±¼øÀ§ +1 ÇÔÀ¸·Î½E
            if (targetCamera != null) targetCamera.Priority = nowCamera.Priority + 1;
        }
    }
}
