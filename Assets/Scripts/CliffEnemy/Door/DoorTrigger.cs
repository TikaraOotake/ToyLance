using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // ｡・ｾﾀ ﾀ・ｯ

/// ﾇﾃｷｹﾀﾌｾ銧｡ 'Door' ｾﾕｿ｡ ｴ・ｸｸ・ﾅｬｸｮｾ・ｾﾀﾀｸｷﾎ ﾀﾌｵｿ
[RequireComponent(typeof(Collider2D))]
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] string clearSceneName = "Clear";   // Build Settingsｿ｡ ｵ﨧ﾏﾇﾑ ﾀﾌｸｧ

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (Application.CanStreamedLevelBeLoaded(clearSceneName))
        {
            SceneManager.LoadScene(clearSceneName);
        }
        else
        {
            Debug.LogError($"シーン名 '{clearSceneName}' は存在しません。");
        }
    }
}
