using UnityEngine;
using System.Collections;

public class Temple : MonoBehaviour {
    public float ritualMeter;
    public int worshiperCounter;
	// Use this for initialization
	void Start ()
    {
        worshiperCounter = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //ritualMeter += (worshiperCounter * 0.01f);
	}

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Worshiper"))
        {
            ritualMeter += 0.01f;
        }
    }
}
