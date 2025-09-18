using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool IsActive = false;

    [SerializeField] GameObject SoftFlashPrefab;

    [SerializeField] Animator _anim;//

    private SEManager _seManager;

    [SerializeField]
    private Effecter effecter;
    void Start()
    {
        _anim = GetComponent<Animator>();

        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SEの取得に失敗");
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
        if (IsActive) return;//既に有効状態なので終了

        if (collision.tag == "Player")
        {
            Debug.Log("チェックポイント設定");

            SetActive(true);
            GameManager_01.SetCheckPoint(this.gameObject);

            //通過音
            _seManager.PlaySE("checkpoint");

            if (SoftFlashPrefab != null)
            {
                Instantiate(SoftFlashPrefab, transform.position, Quaternion.identity);
            }

            if (effecter != null)
            {
                effecter.GenerateEffect();
            }
        }
    }
}
