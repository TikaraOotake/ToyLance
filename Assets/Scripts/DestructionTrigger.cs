using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructionTrigger : MonoBehaviour
{
    [Header("�ı� ��� ����")]
    // �ı��� ����� �Ǵ� '���̴�' Ÿ�ϸ��Դϴ�.
    public Tilemap targetTilemap;

    // �ı��� Ÿ���� ��ġ�� ��� �ִ� '������ �ʴ�' ����ũ Ÿ�ϸ��Դϴ�.
    public Tilemap destructionMaskTilemap;

    [Header("�ı� �ɼ�")]
    // ���߰�: �� üũ�ڽ��� �Ѹ� Ÿ���� �ı��˴ϴ�.
    public bool destroyTiles = true;

    // ���߰�: �� üũ�ڽ��� �Ѹ� Ʈ���� ������Ʈ �ڽ��� �ı��˴ϴ�.
    public bool destroySelf = true;

    public void TriggerDestruction()
    {
        // �ڼ���: destroyTiles�� true�̰� Ÿ�ϸ��� ����Ǿ� ���� ���� Ÿ�� �ı� ������ �����մϴ�.
        if (destroyTiles && targetTilemap != null && destructionMaskTilemap != null)
        {
            // destructionMaskTilemap�� ��� ���� �ִ� ��� Ÿ�� ��ġ�� ��ȸ�մϴ�.
            foreach (var pos in destructionMaskTilemap.cellBounds.allPositionsWithin)
            {
                // ���� ����ũ Ÿ�ϸ��� �ش� ��ġ�� Ÿ���� �ִٸ�,
                if (destructionMaskTilemap.HasTile(pos))
                {
                    // '���̴�' targetTilemap���� ������ ��ġ�� Ÿ���� �����մϴ�.
                    targetTilemap.SetTile(pos, null);
                }
            }
        }
        else if (destroyTiles)
        {
            Debug.LogWarning("Ÿ���� �ı��ϵ��� ����������, Target Tilemap �Ǵ� Destruction Mask Tilemap�� ������� �ʾҽ��ϴ�.");
        }

        // �ڼ���: destroySelf�� true�� ���� �ڽ��� �ı��մϴ�.
        if (destroySelf)
        {
            Destroy(gameObject);
        }
    }
}
