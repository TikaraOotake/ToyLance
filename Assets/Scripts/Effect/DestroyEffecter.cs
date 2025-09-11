using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffecter : MonoBehaviour
{
    [SerializeField] private GameObject EffectPrefab;

    [SerializeField] private int SpawnRate_Min;
    [SerializeField] private int SpawnRate_Max;

    private void OnDestroy()
    {
        //�G�t�F�N�g�̐���
        if (EffectPrefab != null)
        {
            int SpawnNam = Random.Range(SpawnRate_Min, SpawnRate_Max);
            for (int i = 0; i < 7; ++i)
            {
                Instantiate(EffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
