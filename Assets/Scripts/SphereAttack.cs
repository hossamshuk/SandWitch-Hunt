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
	// Use this for initialization
	void Start ()
    {
        attackForce = 0;
        myRigidbody = this.GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        StartCoroutine(EnergyCharging());
        energyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<Slider>();
        chargeBar = GameObject.FindGameObjectWithTag("ChargeBar").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        chargeBar.value = attackForce;
	    if(Input.GetMouseButtonDown(0) && currentEnergy == maxEnergy)
        {
            StartCoroutine("ChargeAttack");
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.useGravity = false;

        }
        if((Input.GetMouseButtonUp(0) || attackForce >= maxForce) && currentEnergy == maxEnergy)
        {
            StopCoroutine("ChargeAttack");
            myRigidbody.useGravity = true;
            myRigidbody.AddForce(this.transform.forward * (attackForce + 10), ForceMode.Impulse);
            currentEnergy = maxEnergy - attackForce;
            attackForce = 0;


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
            Plane playerPlane = new Plane(Vector3.up, ground.transform.position + new Vector3(0, .5f, 0));
            // Generate a ray from the cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.
            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            //   then find the point along that ray that meets that distance.  This will be the point
            //   to look at.
            float hitdist = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

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
}
