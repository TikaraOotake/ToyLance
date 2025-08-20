using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Parallax : MonoBehaviour
{
    public float parallaxFactor = 0.1f;     // 0(하늘) ~ 1(가장 앞)

    Transform cam; Vector3 prevCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
        prevCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - prevCamPos;
        // X 축만(플랫폼러) : 필요하면 Y 도 포함
        transform.position += delta * (1 - parallaxFactor);
        prevCamPos = cam.position;
    }
}
