using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour
{

    [HideInInspector]
    public bool bMatchActive = false;
    public static GameManager Instance = null;

    public static bool host;
    public static string ip;

    public void Start()
    {
        //Debug.Log("wewee");
        //if (Instance != this)
        //    return;

    }

    void Awake()
    {
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{


        Debug.Log("kekeekee");
        if (Application.loadedLevel == 1)
        {
            if (host)
            {
                NetworkManager.singleton.StartServer();
            }
            else
            {
                NetworkManager.singleton.networkAddress = GameManager.ip;
                NetworkManager.singleton.StartClient();
            }
        }


            Instance = this;
            bMatchActive = false;
            Time.timeScale = 0;

            //DontDestroyOnLoad(this.gameObject);
        //}
    }

    [Server]
    void StartGame()
    {
        bMatchActive = true;
        Time.timeScale = 1;
        RpcStartGame();
    }

    void Update()
    {
        if (isServer && Input.GetKeyUp(KeyCode.F5) && !bMatchActive && TeamManager.Instance && TeamManager.Instance.teamOneCounter > 0 && TeamManager.Instance.teamTwoCounter > 0)
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
