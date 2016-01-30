using UnityEngine;
using System.Collections;

public class SphereAttack : MonoBehaviour {
    Rigidbody myRigidbody;
    public float attackForce;
    public float maxForce;
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
            Debug.Log("Started the coroutine");
        }
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Stopped the coroutine");
            StopCoroutine("ChargeAttack");
            myRigidbody.AddForce(this.transform.forward * attackForce, ForceMode.Impulse);
            attackForce = 0;
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
