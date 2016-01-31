using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if(GameManager.host)
        {
            NetworkManager.singleton.StartHost();
        }
        else
        {
            NetworkManager.singleton.networkAddress = GameManager.ip;
            NetworkManager.singleton.StartClient();
        }
	}
	
}
