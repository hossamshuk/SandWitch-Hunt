using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour {
	
	[HideInInspector]
	public bool bMatchActive = false;
	public static GameManager Instance = null;

    public static bool host;
    public static string ip;

	Temple[] Temples;
	
	void Awake () {
		Instance = this;
		bMatchActive = false;
		Time.timeScale = 0;
		Temples = FindObjectsOfType<Temple>();
	}

	
	[Server]
	void StartGame()
	{
		bMatchActive = true;
		Time.timeScale = 1;
		RpcStartGame();
	}

	void Update ()
	{
		if(isServer && Input.GetKeyUp(KeyCode.F5) && !bMatchActive && TeamManager.Instance && TeamManager.Instance.teamOneCounter > 0 && TeamManager.Instance.teamTwoCounter > 0)
		{
			StartGame();
		}
		if(isServer && bMatchActive)
		{
			CheckGameEnd();
		}
	}
	
	[Server]
	void CheckGameEnd()
	{
		foreach(Temple temple in Temples)
		{
			if(temple.ritualMeter >= 100)
			{
				EndMatch(temple.name + " Wins!!");
				break;
			}
		}
	}
	
	[Server]
	void EndMatch(string msg)
	{
		bMatchActive = false;
		Time.timeScale = 0;
		RpcEndMatch(msg);
	}
	
	[ClientRpc]
	public void RpcEndMatch(string msg)
	{
		print("Got Message: " + msg);
		if(FindObjectOfType<UIController>())
			FindObjectOfType<UIController>().DisplayGameEndMessage(msg);
	}
	
	[ClientRpc]
	public void RpcStartGame()
	{
		bMatchActive = true;
		Time.timeScale = 1;
		
	}
}
