using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
    public Rigidbody myRigidbody;
    public float speed;
    public float jumpForce;
    public bool isGrounded;
	// Use this for initialization
	void Start ()
    {
        isGrounded = true;
        myRigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetAxis("Vertical") > 0.1)
        {
            this.myRigidbody.MovePosition(this.transform.position + this.transform.forward * Time.deltaTime * speed);
        }
        else if(Input.GetAxis("Vertical") < -0.1)
        {
            this.myRigidbody.MovePosition(this.transform.position + this.transform.forward * Time.deltaTime * -speed);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Space");
            this.myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
	}

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}
