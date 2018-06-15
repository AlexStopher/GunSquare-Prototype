using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioMain;
	// Use this for initialization
	void Start ()
    {
        audioMain = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlaySound(AudioClip sound)
    {
        audioMain.clip = sound;
        audioMain.PlayOneShot(sound);
    }
}
