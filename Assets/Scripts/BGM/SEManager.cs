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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.Log("audioSourceÇÃéÊìæÇ…é∏îs");

        seDictionary = new Dictionary<string, AudioClip>();
        foreach(var se in seDatas)
        {
            if(!seDictionary.ContainsKey(se.seName))
            {
                seDictionary.Add(se.seName, se.seClip);
            }
            else
            {
                Debug.Log("SEÇ™èdï°");
            }
        }
    }

    
    public void PlaySE(string seName)
    {
        if (audioSource == null) Debug.Log("audioSourceÇ™null");

        if (seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("SE not found");
        }
    }

    public void PlaySE(string seName, Vector2 targetObject)
        {
            if(Collision_Manager.IsPointInsideCollider(Camera.main.GetComponent<Collider2D>(), targetObject))
            {
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
