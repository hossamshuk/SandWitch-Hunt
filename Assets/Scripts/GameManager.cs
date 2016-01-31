using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject PlayerPrefab;
	public Transform PlayerSpawnPoint;
	
	// Use this for initialization
	void Start () {
		StartGame();
	}
	
	void StartGame()
	{
		if(PlayerPrefab && PlayerSpawnPoint)
			Instantiate(PlayerPrefab, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
