using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class seData
{
    public string seName;
    public AudioClip seClip;
}

public class SEManager : MonoBehaviour
{
    [SerializeField]
    private seData[] seDatas;

    private Dictionary<string, AudioClip> seDictionary;
    private AudioSource audioSource;

    public static SEManager instance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.Log("audioSource‚Ìæ“¾‚É¸”s");

        seDictionary = new Dictionary<string, AudioClip>();
        foreach(var se in seDatas)
        {
            if(!seDictionary.ContainsKey(se.seName))
            {
                seDictionary.Add(se.seName, se.seClip);
            }
            else
            {
                Debug.Log("SE‚ªd•¡");
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

    //w’è‚µ‚½SE‚ğÄ¶‚·‚é
    public void PlaySE(string seName)
    {
        if (audioSource == null) Debug.Log("audioSource‚ªnull");

        //SE–¼‚É‘Î‰‚·‚éAudioClip‚ğæ“¾
        if (seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            //Ä¶
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SE not found");
        }
    }

    //‰æ–Ê“à‚É‚ ‚éSE‚ğÄ¶‚·‚é
    public void PlaySE(string seName, Vector2 targetObject)
    {
        //‰æ–Ê“à‚É‚ ‚éê‡
        if(Collision_Manager.IsPointInsideCollider(Camera.main.GetComponent<Collider2D>(), targetObject))
        {
            //Ä¶
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
