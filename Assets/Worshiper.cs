using UnityEngine;
using System.Collections;

public class Worshiper : MonoBehaviour {
    public int health;
    public int maxHealth;
    public bool isAlive;
    public float respawnTime;
    public Animator myAnim;
	// Use this for initialization
	void Start ()
    {
        health = maxHealth;
        isAlive = true;
        myAnim = this.GetComponent<Animator>();
        respawnTime = 10;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(health == 0)
        {
            myAnim.enabled = false;
            Invoke("Die", 4);
        }
	    if(!isAlive)
        {
            DisableWorshiper();
            StartCoroutine(Respawn());
        }
	}
    public void Die()
    {
        isAlive = false;
        //myAnim.enabled = true;
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        enableWorshiper();
    }
    

    public void DisableWorshiper()
    {
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<Collider>().enabled = false;


    }

    public void enableWorshiper()
    {
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
        this.gameObject.transform.position = this.gameObject.GetComponentInParent<Transform>().position;
        isAlive = true;
        health = maxHealth;

    }
    

}
