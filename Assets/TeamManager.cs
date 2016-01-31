using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TeamManager : NetworkBehaviour{

    [SyncVar]
    public int teamOneCounter, teamTwoCounter;
    
	public static TeamManager Instance = null;


	// Use this for initialization
	void Start ()
    {
	    Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void RegisterSelf(int team)
    {
        if(team == 1)
        {
            teamOneCounter++;
            Debug.Log("team one counter: " + teamOneCounter);
        }
        else
        {
            teamTwoCounter++;
            Debug.Log("team two counter: " + teamTwoCounter);
        }
    }
    [ClientRpc]
    public void RpcRegisterSelf(int team)
    {
        if (team == 1)
        {
            teamOneCounter++;
            Debug.Log("team one counter: " + teamOneCounter);
        }
        else
        {
            teamTwoCounter++;
            Debug.Log("team two counter: " + teamTwoCounter);
        }
    }
   
}
