using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System.Text;
using MyClient;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    AsyncClient asyncClient;

    public InputField userName;
    public InputField password;
    public GameObject messageGO;
    public LoginState state = LoginState.Input;

    private LoginState lastState = LoginState.Input;
	// Use this for initialization
	void Start ()
    {
        if (NetClient.asyncClient==null)
        {
            IPAddress ep = IPAddress.Parse(NetClient.serverIP);
            asyncClient = new AsyncClient(new IPAddress[] { ep }, 8421, null);
            NetClient.asyncClient = asyncClient;
            asyncClient.ServerConnected += NetClient.ProcessConnected;
            asyncClient.ServerExceptionOccurred += NetClient.ProcessNetError;
            asyncClient.ServerDisconnected += NetClient.ProcessNetError;
            asyncClient.DatagramReceived += NetClient.ProcessReceive;
            asyncClient.Connect();
        }
        else
        {
            NetClient.player = null;
            NetClient.clientId = new Guid();
        }
        

        userName.text = "cc";
        password.text = "11";
	}
	
   

	// Update is called once per frame
	void Update () 
    {
        if (lastState!=state)
        {
            if (state== LoginState.Failed)
            {
                password.text = "";
                messageGO.SetActive(true);
                Button btn = messageGO.transform.GetChild(0).GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.transform.GetChild(0).GetComponent<Text>().text = "登录失败！\n请检查用户名或密码！";
                btn.onClick.AddListener(() =>
                {
                    messageGO.SetActive(false);
                    state = LoginState.Input;
                });
            }
            else if (state== LoginState.Sucess)
            {
                Application.LoadLevel(1);
            }
            lastState = state;
        }
	}

    

    public void Login_Method()
    {
        if (NetClient.isConnected)
        {
            var N= NetClient.GetStandJson();
            N["MsgType"] = ((int)MsgType.Login).ToString();
            N["clientId"] = NetClient.clientId.ToString();
            N[MsgType.Login.ToString()]["userName"] = userName.text;
            N[MsgType.Login.ToString()]["password"] = password.text;
            Debug.Log("Send: "+N.ToString());
            NetClient.SendJson(N);
            NetClient.OnLogin_Response = Login_Response;
            state = LoginState.Logining;
        }
    }

    public void Login_Response(JSONNode N)
    {
        Debug.Log("Login_Response");
        if (NetClient.isConnected)
        {
            bool result = bool.Parse(JSON.GetStr(N["Login_Response"]["Result"].ToString()));
            if (result)
            {
                Debug.Log("登录成功了");
                GetPlayerInfo(N);
                state = LoginState.Sucess;
            } 
            else
            {
                Debug.Log("登录失败了");
                state = LoginState.Failed;
                
            }
            NetClient.OnLogin_Response = null;
        }
        
    }



    public void GetPlayerInfo(JSONNode N)
    {
        Debug.Log("GetPlayerInfo");
        NetClient.player = new Player();
        Player player = NetClient.player;
        player.clientId = NetClient.clientId;
        player._Session = asyncClient._Session;
        player.GetFromJson(MsgType.Login_Response.ToString(), N);
        Debug.Log(MsgType.Login_Response.ToString());
        //778858258wang
    }

    public enum LoginState
    {
        Input=0,
        Logining=1,
        Sucess=2,
        Failed=3,
    }
	

}
