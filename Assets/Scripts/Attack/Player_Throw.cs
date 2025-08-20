using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Throw : MonoBehaviour
{
    [SerializeField] GameObject spearPrefab;
    [SerializeField] float throwCool = 0.4f;
    [SerializeField] float spearSpeed = 12f;

    float nextTime;
    SpriteRenderer sr;
    private Animator anim; // Animator 변수 추가

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // Animator 컴포넌트 찾아오기
    }

    void Update()
    {
        // 근접 공격 애니메이션("Attack_1") 중에는 창 던지기 불가
        if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            return;
        }

        if (!Input.GetButtonDown("Ranged") || Time.time < nextTime) return;
        nextTime = Time.time + throwCool;

        int dir = sr.flipX ? -1 : 1;
        Vector3 spawnPos = transform.position + Vector3.right * dir * 0.60f + Vector3.up * 0.05f;

        // ★★★ 여기가 수정된 로직입니다 ★★★
        Quaternion rot;
        if (dir > 0) // 오른쪽을 볼 때 (dir = 1)
        {
            // Z축 90도로 회전
            rot = Quaternion.Euler(0, 0, 90);
        }
        else // 왼쪽을 볼 때 (dir = -1)
        {
            // Z축 -90도로 회전 (기존과 동일)
            rot = Quaternion.Euler(0, 0, -90);
        }

        GameObject go = Instantiate(spearPrefab, spawnPos, rot);

        if (go.TryGetComponent(out SpearProjectile stick))
        {
            stick.Init(dir, spearSpeed);
        }
    }
}

