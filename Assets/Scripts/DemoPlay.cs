using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //フェードさせるコルーチンを開始
        StartCoroutine(BlindFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //フェードさせるコルーチン
    IEnumerator BlindFade()
    {
        //30秒待つ
        yield return new WaitForSeconds(30.0f);
        GameManager_01.SetBlindFade(true);

        float duration = 2.0f;
        float time = 0.0f;
        float startVolume = BGMManager.instance.baseVolume;

        //音量をフェードする
        while (time < duration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0.0f, time / duration);
            BGMManager.instance.baseVolume = newVolume;
            yield return null;
        }

        //音量を0に
        BGMManager.instance.baseVolume = 0.0f;

        //デモを再生させるコルーチンを開始
        StartCoroutine(PlayDemo());
    }

    //デモを再生させるコルーチン
    IEnumerator PlayDemo()
    {
        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Demo");
    }
}
