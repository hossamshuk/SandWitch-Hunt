using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public float differenceInX, differenceInY, differenceInZ;
    public GameObject Player;
	// Use this for initialization
	void Start ()
    {
        differenceInX = Player.transform.position.x - this.transform.position.x;
        differenceInY = Player.transform.position.y - this.transform.position.y;
        differenceInZ = Player.transform.position.z - this.transform.position.z;

    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(Player.transform.position.x - differenceInX, Player.transform.position.y - differenceInY, Player.transform.position.z - differenceInZ);
	}
}
