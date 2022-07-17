using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] audioClip;
    public AudioSource audioSource;
    public float musicTimer;
    private float i, j;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        i = j = 1;

    }

    // Update is called once per frame
    void Update()
    {
        WaitForMusic();
    }
    void WaitForMusic()
    {
        i -= (Time.deltaTime / GameplayController.instance.timeMultiple);
        if(i <= 0)
        {
            musicTimer -= 1;
            i = 1;
        }
        if(musicTimer <= 0)
        {
            int i = Random.Range(0, 4);
            audioSource.clip = audioClip[i];
            audioSource.Play();
            musicTimer = Random.Range(audioClip[i].length + 20, 
                audioClip[i].length + 40);
        }
    }

}//class

























