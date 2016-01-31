using UnityEngine;
using System.Collections;

public class TrapAudio : MonoBehaviour {
    public AudioSource myAudio;
	// Use this for initialization
	void Start () {
        myAudio = this.GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
