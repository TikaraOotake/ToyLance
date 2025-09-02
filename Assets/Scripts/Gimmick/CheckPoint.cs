using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool IsActive = false;

    [SerializeField] Animator _anim;//

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive) return;//���ɗL����ԂȂ̂ŏI��

        if (collision.tag == "Player")
        {
            Debug.Log("�`�F�b�N�|�C���g�ݒ�");
            IsActive = true;
            GameManager_01.SetCheckPoint(this.gameObject);
        }
    }
}
