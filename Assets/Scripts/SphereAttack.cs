using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SphereAttack : MonoBehaviour {
    Rigidbody myRigidbody;
    public float attackForce;
    public float maxForce;
    public bool isGrounded;
    public float jumpForce;
    public GameObject ground;
    public float maxEnergy;
    public float currentEnergy;
    public Slider chargeBar;
    public Slider energyBar;
    bool mouseDown, mouseUp;
    public GameObject bloodSplatter;
    public GameObject mouseTarget;
	// Use this for initialization
	void Start ()
    {
        attackForce = 0;
        myRigidbody = this.GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        StartCoroutine(EnergyCharging());
        energyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<Slider>();
        chargeBar = GameObject.FindGameObjectWithTag("ChargeBar").GetComponent<Slider>();
        mouseTarget = GameObject.FindGameObjectWithTag("MouseTarget");
	}
	
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            mouseUp = true;
        }
        
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        chargeBar.value = attackForce;
	    if(mouseDown && currentEnergy == maxEnergy)
        {
            StartCoroutine("ChargeAttack");
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.useGravity = false;
            mouseDown = false;

        }
        if((mouseUp || attackForce >= maxForce) && currentEnergy == maxEnergy)
        {
            StopCoroutine("ChargeAttack");

            myRigidbody.AddForce(this.transform.forward * (attackForce + 10), ForceMode.Impulse);
            myRigidbody.useGravity = true;

            currentEnergy = maxEnergy - attackForce;
            attackForce = 0;
            mouseUp = false;


        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jumped");
        }
        energyBar.value = currentEnergy;
	}


    public IEnumerator ChargeAttack()
    {
        while(true)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1<<LayerMask.NameToLayer("Ground")))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = hitInfo.point;

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                // Smoothly rotate towards the target point.
                transform.rotation = targetRotation;
            }

            if (attackForce < maxForce)
            {
                attackForce += 0.5f;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator EnergyCharging()
    {
        while(true)
        {
            if(currentEnergy < maxEnergy)
            {
                currentEnergy += 0.5f;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Worshiper") && myRigidbody.velocity.magnitude > 20)
        {
            other.gameObject.GetComponent<Worshiper>().health--;

            RaycastHit hit;
            if (Physics.Raycast(other.gameObject.transform.position, Vector3.down, out hit))
            {
                Instantiate(bloodSplatter, hit.point, Quaternion.Euler( hit.normal));
            }

        }
    }
}
