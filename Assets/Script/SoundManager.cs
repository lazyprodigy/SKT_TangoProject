using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
    public static SoundManager Instance;
    AudioSource MyAudio;

    public AudioClip[] Clips;

    void Awake() 
    {
        Instance = GetComponent<SoundManager>();
        MyAudio = GetComponent<AudioSource>();
	}

    public void PlaySound(int index)
    {
        MyAudio.clip = Clips[index];        
        MyAudio.Play();
    }

	void Update () 
    {
	
	}
}
