using UnityEngine;
using System.Collections;

public class Temple : MonoBehaviour {
    public float ritualMeter;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
    
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Worshiper"))
        {
            ritualMeter += 0.01f;
        }
    }
}
