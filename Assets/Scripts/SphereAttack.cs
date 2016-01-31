using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
public class SphereAttack : NetworkBehaviour {
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
    TeamManager teamManager;
    public Material blueMaterial, redMaterial;

    [SyncVar]
    public int myTeam;
	
	public delegate void AssignTeamColors();
	[SyncEvent]
	public event AssignTeamColors EventAssignTeamColors;
	
	void Start ()
    {
        attackForce = 0;
        myRigidbody = this.GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        StartCoroutine(EnergyCharging());
        energyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<Slider>();
        chargeBar = GameObject.FindGameObjectWithTag("ChargeBar").GetComponent<Slider>();
        mouseTarget = GameObject.FindGameObjectWithTag("MouseTarget");
		
		transform.Translate(Vector3.up * 10);
	}
	
	public void ShouldAssignTeamColors()
	{
		if (myTeam == 1)
        {
            this.gameObject.name = "Player 1";
            GetComponent<Renderer>().material = blueMaterial;
        }
        else if (myTeam == 2)
        {
            this.gameObject.name = "Player 2";
            GetComponent<Renderer>().material = redMaterial;
        }
	}
	
	[ClientRpc]
	public void RpcAssignTeamColors()
	{
		if (myTeam == 1)
        {
            this.gameObject.name = "Player 1";
            GetComponent<Renderer>().material = blueMaterial;
        }
        else if (myTeam == 2)
        {
            this.gameObject.name = "Player 2";
            GetComponent<Renderer>().material = redMaterial;
        }
	}
	
	void Awake()
    {
        ground = GameObject.Find("Ground");
        chargeBar = GameObject.Find("Chargebar").GetComponent<Slider>();
        energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
		
		EventAssignTeamColors += new AssignTeamColors(ShouldAssignTeamColors);
    }

    public override void OnStartLocalPlayer()
    {
        teamManager = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
        if (teamManager.teamOneCounter <= teamManager.teamTwoCounter)
        {
            myTeam = 1;
            CmdRegsiterSelf(1);
            Debug.Log("Registered self as " + myTeam);
        }
        else if (teamManager.teamOneCounter > teamManager.teamTwoCounter)
        {
            myTeam = 2;
            CmdRegsiterSelf(2);
            Debug.Log("Registered self as " + myTeam);
        }
		if (myTeam == 1)
        {
            this.gameObject.name = "Player 1";
            GetComponent<Renderer>().material = blueMaterial;
			print(gameObject.name);
        }
        else
        {
            this.gameObject.name = "Player 2";
            GetComponent<Renderer>().material = redMaterial;
        }
		CmdCallAssignTeamColors();
    }
	
    [Command]
    public void CmdRegsiterSelf(int team)
    {
		myTeam = team;
        if (team == 1)
        {
            teamManager.RegisterSelf(1);
            Debug.Log("team one counter: " );
        }
        else
        {
            teamManager.RegisterSelf(2);
            Debug.Log("team two counter: ");
        }
    }
	
	[Command]
	public void CmdCallAssignTeamColors()
	{
		EventAssignTeamColors();
	}

    void Update()
    {
        if (!isLocalPlayer)
            return;
        if(Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            mouseUp = true;
        }
        
    }
	void FixedUpdate ()
    {
        if (!isLocalPlayer)
            return;
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
        //Divide temple prefab into two and give team numbers to differentiate
        if(myTeam != 0 && other.gameObject.CompareTag("Worshiper"+myTeam) && other.impulse.magnitude > 8)
        {
            if(other.gameObject.GetComponent<Worshiper>())
				other.gameObject.GetComponent<Worshiper>().TakeDamage(1);
            RaycastHit hit;
            if (Physics.Raycast(other.gameObject.transform.position, Vector3.down, out hit))
            {
                Instantiate(bloodSplatter, hit.point, Quaternion.Euler( hit.normal));
            }

        }
    }
}
