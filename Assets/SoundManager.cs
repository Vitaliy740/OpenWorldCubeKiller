using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource MusicSource;
    public AudioClip BackGroundMusic;
    public AudioClip DeathSound;
    public AudioClip HealSound;
    public AudioClip PickUpSound;
    // Start is called before the first frame update
    void Start()
    {
        ResetMusic();
    }
    public void ResetMusic()
    {
        MusicSource.clip = BackGroundMusic;
        MusicSource.Play();
    }
    public void PlaySound(AudioClip clip) 
    {
        SFXSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
