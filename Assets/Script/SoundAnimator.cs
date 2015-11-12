using UnityEngine;
using System.Collections;

public class SoundAnimator : MonoBehaviour 
{
    
    AudioSource CompAudio;
    
	void Start () 
    {
        CompAudio = GetComponent<AudioSource>();    
	}

    public void PlaySound(AudioClip clips)
    {
        CompAudio.clip = clips;
        CompAudio.Play();
    }

	void Update () 
    {
	
	}
}
