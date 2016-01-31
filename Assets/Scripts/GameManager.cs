using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour {
	
	[HideInInspector]
	public bool bMatchActive = false;
	public static GameManager Instance = null;
	
	void Start () {
		Instance = this;
		bMatchActive = false;
		Time.timeScale = 0;
	}
	
	[Command]
	public void CmdPrintMeThis()
	{
		print("asdasdasdasdasd");
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
		if(isServer && Input.GetKeyUp(KeyCode.F5) && !bMatchActive)
		{
			StartGame();
		}
		if(isServer)
			print("Server");
		if(isClient && !isServer)
		{
			print("Client");
			CmdPrintMeThis();
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
