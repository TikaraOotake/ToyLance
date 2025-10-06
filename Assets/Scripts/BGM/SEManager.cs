using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class seData
{
    public string seName;       //SE��
    public AudioClip seClip;    //SE��
}

public class SEManager : MonoBehaviour
{
    [SerializeField]
    private seData[] seDatas;

    private Dictionary<string, AudioClip> seDictionary;
    private AudioSource audioSource;

    //�V���O���g��
    public static SEManager instance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.Log("audioSource�̎擾�Ɏ��s");

        seDictionary = new Dictionary<string, AudioClip>();
        foreach(var se in seDatas)
        {
            if(!seDictionary.ContainsKey(se.seName))
            {
                seDictionary.Add(se.seName, se.seClip);
            }
            else
            {
                Debug.Log("SE���d��");
            }
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //�w�肵��SE���Đ�����
    public void PlaySE(string seName)
    {
        if (audioSource == null) Debug.Log("audioSource��null");

        //SE���ɑΉ�����AudioClip���擾
        if (seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            //�Đ�
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SE not found");
        }
    }

    //��ʓ��ɂ���SE���Đ�����
    public void PlaySE(string seName, Vector2 targetObject)
    {
        //��ʓ��ɂ���ꍇ
        if(Collision_Manager.IsPointInsideCollider(Camera.main.GetComponent<Collider2D>(), targetObject))
        {
            //�Đ�
            PlaySE(seName);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
