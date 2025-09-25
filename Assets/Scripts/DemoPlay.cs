using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlindFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BlindFade()
    {
        yield return new WaitForSeconds(30.0f);
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

        StartCoroutine(PlayDemo());
    }

    IEnumerator PlayDemo()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Demo");
    }
}
