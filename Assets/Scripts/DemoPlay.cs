using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�t�F�[�h������R���[�`�����J�n
        StartCoroutine(BlindFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�t�F�[�h������R���[�`��
    IEnumerator BlindFade()
    {
        //30�b�҂�
        yield return new WaitForSeconds(30.0f);
        GameManager_01.SetBlindFade(true);

        float duration = 2.0f;
        float time = 0.0f;
        float startVolume = BGMManager.instance.baseVolume;

        //���ʂ��t�F�[�h����
        while (time < duration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0.0f, time / duration);
            BGMManager.instance.baseVolume = newVolume;
            yield return null;
        }

        //���ʂ�0��
        BGMManager.instance.baseVolume = 0.0f;

        //�f�����Đ�������R���[�`�����J�n
        StartCoroutine(PlayDemo());
    }

    //�f�����Đ�������R���[�`��
    IEnumerator PlayDemo()
    {
        //0.5�b�҂�
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Demo");
    }
}
