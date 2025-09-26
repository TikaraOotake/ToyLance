using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject SpawnObjectPrefab; // ��������I�u�W�F�N�g
    [SerializeField] private float SpawnCycleTime = 1.0f; // �X�|�[���Ԋu
    [SerializeField] private float AddRandamCycleTimeScale = 0.0f; // �X�|�[���Ԋu�Ƀ����_���ȕ␳�l���������
    private float SpawnCycleTimer;

    [SerializeField] private Vector2 RandamSpawnPosOffset;//�����_���ŃX�|�[���n�_�����炷��

    [SerializeField]
    private Vector2 SpawnVel;//���������I�u�W�F�N�g�ɉ�����(���W�b�g�{�f�B������ꍇ)

    [SerializeField] private bool IsSpawning = true; // �����J�n�t���O
    private bool IsSpawning_old;

    enum SpawnMode
    {
        FullAuto, // �������Ő�����������
        SemiAuto, // ��񂾂�����
    }
    [SerializeField] SpawnMode mode;

    [SerializeField] private SwitchButton[] SwitchList; // �X�C�b�`

    [Header("�����ݒ�")]
    [SerializeField] private int MaxObjects = 10;   // �ő吔 (0�ȉ��Ȃ疳����)
    [SerializeField] private float LifeTime = 10.0f;  // ���� (�b, 0�ȉ��Ȃ疳����)

    // ���������I�u�W�F�N�g�Ɛ������Ԃ��L�^
    private List<(GameObject obj, float spawnTime)> spawnedObjects = new List<(GameObject, float)>();


    void Start()
    {
        SpawnCycleTimer = SpawnCycleTime + Random.Range(0.0f, AddRandamCycleTimeScale);
    }

    void Update()
    {
        // �X�C�b�`��Ԃ��m�F
        
        if (SwitchList != null && SwitchList.Length > 0)
        {
            IsSpawning = true;
            for (int i = 0; i < SwitchList.Length; ++i)
            {
                if (SwitchList[i] != null && !SwitchList[i].GetSwitchFlag())
                {
                    IsSpawning = false;
                }
            }
        }

        // ���[�h���Ƃ̐�������
        if (mode == SpawnMode.FullAuto)
        {
            if (IsSpawning) Update_Timer(ref SpawnCycleTimer);

            if (SpawnCycleTimer <= 0.0f)
            {
                SpawnObject();
                SpawnCycleTimer = SpawnCycleTime + Random.Range(0.0f, AddRandamCycleTimeScale);
            }
        }
        else if (mode == SpawnMode.SemiAuto)
        {
            if (IsSpawning && IsSpawning != IsSpawning_old)
            {
                SpawnObject();
            }
        }

        // �O���Ŕj�󂳂ꂽ�I�u�W�F�N�g������؂���폜
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            var entry = spawnedObjects[i];

            // ���łɔj�󂳂�Ă���
            if (entry.obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            // �����`�F�b�N
            if (LifeTime > 0 && Time.time - entry.spawnTime >= LifeTime)
            {
                Destroy(entry.obj);
                spawnedObjects.RemoveAt(i);
            }
        }
        
        //�O�t���[���̃t���O�̏�Ԃ��L�^
        IsSpawning_old = IsSpawning;
    }

    private void SpawnObject()
    {
        if (SpawnObjectPrefab == null) return;

        //�o�����W��ݒ�
        Vector2 SpawnPos = transform.position;
        SpawnPos.x += Random.Range(-RandamSpawnPosOffset.x, RandamSpawnPosOffset.x);
        SpawnPos.y += Random.Range(-RandamSpawnPosOffset.y, RandamSpawnPosOffset.y);

        //�I�u�W�F�N�g����
        GameObject newObj = Instantiate(SpawnObjectPrefab, SpawnPos, Quaternion.identity);

        //���W�b�g�{�f�B������ꍇ�͑��x��^����
        Rigidbody2D _rb = newObj.GetComponent<Rigidbody2D>();
        if (_rb)
        {
            _rb.velocity = SpawnVel;//���x����
        }

        //���X�g�ɒǉ�
        spawnedObjects.Add((newObj, Time.time));

        // �ő吔�����ɂ��폜
        if (MaxObjects > 0 && spawnedObjects.Count > MaxObjects)
        {
            var oldest = spawnedObjects[0];
            if (oldest.obj != null) Destroy(oldest.obj);
            spawnedObjects.RemoveAt(0);
        }
    }

    private void Update_Timer(ref float _Timer)
    {
        _Timer = Mathf.Max(0.0f, _Timer - Time.deltaTime);
    }
}
