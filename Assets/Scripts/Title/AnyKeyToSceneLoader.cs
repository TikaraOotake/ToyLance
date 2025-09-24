using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToSceneLoader : MonoBehaviour
{
    [Tooltip("SceneName")]
    public string sceneNameToLoad;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(BlindFade());
        }
    }

    IEnumerator BlindFade()
    {
        yield return null;
        GameManager_01.SetBlindFade(true);

        float duration = 2.0f;
        float time = 0.0f;
        float startVolume = BGMManager.instance.baseVolume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0.0f, time / duration);
            BGMManager.instance.baseVolume = newVolume;
            yield return null;
        }

        BGMManager.instance.baseVolume = 0.0f;

        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
