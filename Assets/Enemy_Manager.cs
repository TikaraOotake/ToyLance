using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    private static Enemy_Manager Enemy_ManagerInstance;

    // �C���X�^���X�̎擾
    public static Enemy_Manager Instance
    {
        get
        {
            if (Enemy_ManagerInstance == null)
            {
                // �V�[������Enemy_Manager�I�u�W�F�N�g���Ȃ��ꍇ�A�V�����쐬����
                Enemy_ManagerInstance = FindObjectOfType<Enemy_Manager>();

                if (Enemy_ManagerInstance == null)
                {
                    GameObject Enemy_Manager = new GameObject("Enemy_Manager");
                    Enemy_ManagerInstance = Enemy_Manager.AddComponent<Enemy_Manager>();
                }
            }
            return Enemy_ManagerInstance;
        }
    }

    //���S�����G�l�~�[���L�^���Ă������X�g
    private List<GameObject> DeadEnemyList = new List<GameObject>();

    public void SetDeadEnemy(GameObject _enemy)
    {
        if (_enemy != null)
        {
            DeadEnemyList.Add(_enemy);

            Debug.Log(_enemy.name + "�����S���X�g�ɓo�^");
        }
    }

    private void Update()
    {
        // ���X�g����납�烋�[�v���āA���X�|�[�����邩�`�F�b�N
        for (int i = DeadEnemyList.Count - 1; i >= 0; i--)
        {
            if (DeadEnemyList[i] != null)
            {
                bool result = false;
                EnemyHealth enemyHealth = DeadEnemyList[i].GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    result = enemyHealth.CheckRespawn();
                }

                if (result == true)
                {
                    DeadEnemyList.RemoveAt(i);//���A�ɐ��������烊�X�g����O��
                }

            }
        }
    }

}
