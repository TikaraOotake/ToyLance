using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    [SerializeField]
    public AudioClip seClip;
    [Range(0f, 1f)]
    public float baseVolume = 1.0f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlaySE()
    {
        if (seClip == null)
        {
            return;
        }
        audioSource.PlayOneShot(seClip, baseVolume);
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
