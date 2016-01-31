using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup menuGroup;
    public CanvasGroup joinGroup;
    public InputField ipField;


    NetworkClient myClient;

    public void Host()
    {
        SetupServer();
        //SetupLocalClient();
    }

    public void JoinMenu()
    {
        StartCoroutine(FadeGroup(menuGroup, true));
        StartCoroutine(FadeGroup(joinGroup, false));
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Join()
    {
        SetupClient(ipField.text);
    }

    public void Back()
    {
        StartCoroutine(FadeGroup(menuGroup, false));
        StartCoroutine(FadeGroup(joinGroup, true));
    }

    public IEnumerator FadeGroup(CanvasGroup cg, bool fadeOut)
    {
        if(!fadeOut)
            cg.gameObject.SetActive(true);
        
        while(true)
        {
            if (fadeOut)
            {
                if (cg.alpha > 0)
                    cg.alpha -= 0.1f;
                else
                {
                    cg.gameObject.SetActive(false);
                    yield break;
                }
            }
            else if (!fadeOut)
            {
                if (cg.alpha < 1)
                    cg.alpha += 0.1f;
                else
                    yield break;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }


    public void SetupServer()
    {
        GameManager.host = true;
        Application.LoadLevel(1);
        //NetworkServer.Listen(4444);
    }
    
    public void SetupClient(string ip)
    {
        GameManager.host = false;
        GameManager.ip = ip;
        Application.LoadLevel(1);
        //myClient = new NetworkClient();
        //myClient.RegisterHandler(MsgType.Connect, OnConnected);
        //myClient.Connect(ip, 7777);
    }

    public void SetupLocalClient()
    {
        GameManager.host = false;
        GameManager.ip = "localhost";
        Application.LoadLevel(1);
        //myClient = ClientScene.ConnectLocalServer();
        //myClient.RegisterHandler(MsgType.Connect, OnConnected);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
        Application.LoadLevel(1);
    }
}
