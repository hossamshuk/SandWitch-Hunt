using UnityEngine;
using System.Collections;

public class ChantingSound : MonoBehaviour {
    public AudioSource myAudio;
	// Use this for initialization
	void Start ()
    {

        myAudio = this.GetComponent<AudioSource>();
        myAudio.pitch = Random.Range(0.8f, 1.3f);
        Invoke("PlayAudio", Random.Range(0.1f, 0.13f));
	}
	
	// Update is called once per frame

    public void PlayAudio()
    {
        myAudio.Play();
    }
    
}
