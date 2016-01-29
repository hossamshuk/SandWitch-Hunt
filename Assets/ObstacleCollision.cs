using UnityEngine;
using System.Collections;

public class ObstacleCollision : MonoBehaviour {
    public Rigidbody myRigidbody;
	// Use this for initialization
	void Start ()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            myRigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
