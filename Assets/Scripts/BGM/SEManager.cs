using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] seClips;
    
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySE(string seName)
    {
        switch(seName)
        {
            case "footsteps":
                audioSource.PlayOneShot(seClips[0]);
                break;

            case "jump":
                audioSource.PlayOneShot(seClips[1]);
                break;
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
