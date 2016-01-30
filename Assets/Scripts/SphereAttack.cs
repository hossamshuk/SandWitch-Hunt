using UnityEngine;
using System.Collections;

public class SphereAttack : MonoBehaviour {
    Rigidbody myRigidbody;
    public float attackForce;
    public float maxForce;
    public bool isGrounded;
    public float jumpForce;
	// Use this for initialization
	void Start ()
    {
        attackForce = 0;
        myRigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine("ChargeAttack");
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.useGravity = false;
            Debug.Log("Started the coroutine");
        }
        if(Input.GetMouseButtonUp(0) || attackForce >= maxForce)
        {
            Debug.Log("Stopped the coroutine");
            StopCoroutine("ChargeAttack");
            myRigidbody.useGravity = true;
            myRigidbody.AddForce(this.transform.forward * attackForce, ForceMode.Impulse);
            attackForce = 0;
            
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jumped");
        }

	}


    public IEnumerator ChargeAttack()
    {
        while(true)
        {
            if (attackForce < maxForce)
            {
                attackForce += 3.5f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
