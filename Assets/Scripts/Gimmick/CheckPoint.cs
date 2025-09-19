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

        //�A�j���[�V�����K�p
        if(_anim)
        {
            _anim.SetBool("IsActive", IsActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive) return;//���ɗL����ԂȂ̂ŏI��

        if (collision.tag == "Player")
        {
            Debug.Log("�`�F�b�N�|�C���g�ݒ�");

            SetActive(true);
            GameManager_01.SetCheckPoint(this.gameObject);

            //�ʉ߉�
            SEManager.instance.PlaySE("checkpoint");

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
