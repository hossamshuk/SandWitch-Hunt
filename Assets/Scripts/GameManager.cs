using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour {
	
	[HideInInspector]
	public bool bMatchActive = false;
	public static GameManager Instance = null;
	
	void Awake () {
		Instance = this;
		bMatchActive = false;
		Time.timeScale = 0;
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
	}
	
	[Server]
	void EndMatch()
	{
		bMatchActive = false;
		Time.timeScale = 0;
	}
	
	[ClientRpc]
	public void RpcStartGame()
	{
		bMatchActive = true;
		Time.timeScale = 1;
	}
}
