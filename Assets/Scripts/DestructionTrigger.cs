using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructionTrigger : MonoBehaviour
{
    [Header("파괴 대상 설정")]
    // 파괴될 대상이 되는 '보이는' 타일맵입니다.
    public Tilemap targetTilemap;

    // 파괴될 타일의 위치를 담고 있는 '보이지 않는' 마스크 타일맵입니다.
    public Tilemap destructionMaskTilemap;

    [Header("파괴 옵션")]
    // ★추가: 이 체크박스를 켜면 타일이 파괴됩니다.
    public bool destroyTiles = true;

    // ★추가: 이 체크박스를 켜면 트리거 오브젝트 자신이 파괴됩니다.
    public bool destroySelf = true;

    public void TriggerDestruction()
    {
        // ★수정: destroyTiles가 true이고 타일맵이 연결되어 있을 때만 타일 파괴 로직을 실행합니다.
        if (destroyTiles && targetTilemap != null && destructionMaskTilemap != null)
        {
            // destructionMaskTilemap의 경계 내에 있는 모든 타일 위치를 순회합니다.
            foreach (var pos in destructionMaskTilemap.cellBounds.allPositionsWithin)
            {
                // 만약 마스크 타일맵의 해당 위치에 타일이 있다면,
                if (destructionMaskTilemap.HasTile(pos))
                {
                    // '보이는' targetTilemap에서 동일한 위치의 타일을 제거합니다.
                    targetTilemap.SetTile(pos, null);
                }
            }
        }
        else if (destroyTiles)
        {
            Debug.LogWarning("타일을 파괴하도록 설정했지만, Target Tilemap 또는 Destruction Mask Tilemap이 연결되지 않았습니다.");
        }

        // ★수정: destroySelf가 true일 때만 자신을 파괴합니다.
        if (destroySelf)
        {
            Destroy(gameObject);
        }
    }
}
