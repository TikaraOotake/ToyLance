using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool IsActive = false;

    [SerializeField] GameObject SoftFlashPrefab;

    [SerializeField] Animator _anim;//

    [SerializeField]
    private Effecter effecter;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetActive(bool _flag)
    {
        IsActive = _flag;

        //アニメーション適用
        if(_anim)
        {
            _anim.SetBool("IsActive", IsActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーのHPを回復させる
        Player_01_Control player = collision.GetComponent<Player_01_Control>();
        if (player != null)
        {
            player.SetPlayerHP(player.GetPlayerHP_Max());

            //キラキラのエフェクトを出す
            if (effecter != null)
            {
                effecter.GenerateEffect();
            }
        }

        if (IsActive) return;//既に有効状態なので終了

        if (collision.tag == "Player")
        {
            Debug.Log("チェックポイント設定");

            SetActive(true);
            GameManager_01.SetCheckPoint(this.gameObject);

            //通過音
            SEManager.instance.PlaySE("checkpoint");

            //明るくするエフェクト
            if (SoftFlashPrefab != null)
            {
                Instantiate(SoftFlashPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
