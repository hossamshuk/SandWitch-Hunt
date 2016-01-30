using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
    Rigidbody myRigidbody;
    public float speed;
    public float maxSpeed;
    public float jumpForce;
    public bool isGrounded;
    Vector3 movementDir;
	// Use this for initialization
	void Start ()
    {
        isGrounded = true;
        myRigidbody = this.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        movementDir.x = movementDir.y = movementDir.z = 0;
        movementDir.z = Input.GetAxis("Vertical")*speed;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            movementDir.y = jumpForce;

        }
        this.myRigidbody.AddForce(movementDir, ForceMode.Impulse);

        if (Mathf.Abs(this.myRigidbody.velocity.z) > maxSpeed)
        {
            movementDir = this.myRigidbody.velocity;
            movementDir.z = maxSpeed*Mathf.Sign(movementDir.z);
            this.myRigidbody.velocity= movementDir;
        }
    }
    float Damp (float speed,float maxSpeed)
    {
        return Mathf.Min(speed, maxSpeed);
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.constraints = RigidbodyConstraints.None;
            Debug.Log(other.contacts[0].point); ;
            if (other.contacts[0].point.y < 1)
            {
                myRigidbody.AddForce((this.transform.position - other.transform.position) * 1000);
            }
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}
