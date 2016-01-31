using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
    Animator myAnim;
    public GameObject player;
    bool isOn;
	// Use this for initialization
	void Start ()
    {
        isOn = true;
        myAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" && isOn)
        {
            isOn = false;
            myAnim.SetTrigger("CloseTrap");
            other.GetComponent<Rigidbody>().isKinematic = true;
            player = other.gameObject;
            Invoke("ReleasePlayer", 4);
            Invoke("ResetTrap", 6);
            
        }

    }

    public void ReleasePlayer()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void ResetTrap()
    {
        isOn = true;
    }
}
