using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	
	public GameObject WinMessage;
	
	void Awake()
	{
		WinMessage.SetActive(false);	
	}
	
	public void DisplayGameEndMessage(string message)
	{
		if(WinMessage && WinMessage.GetComponent<Text>())
		{
			WinMessage.GetComponent<Text>().text = message;
			WinMessage.SetActive(true);
		}
	}
}
