using UnityEngine;
using System.Collections;

public class Worshiper : MonoBehaviour {
    public bool isAlive;
    public float respawnTime;
	// Use this for initialization
	void Start ()
    {
        isAlive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!isAlive)
        {
            DisableWorshiper();
            StartCoroutine(Respawn());
            
        }
	}

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        enableWorshiper();
    }

    public void DisableWorshiper()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;

    }

    public void enableWorshiper()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.transform.position = this.gameObject.GetComponentInParent<Transform>().position;
    }
    

}
