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
using System.Collections.Generic;

public class NetClient {
    public static NetClient Instance = null;

    public static NetClientState state = NetClientState.Loading;
    //public static string serverIP = "127.0.0.1";
    public static string serverIP = "192.168.137.2";
    public static int port = 8421;
    public static Player player = null;
    public static bool isConnected = false;
    public static AsyncClient asyncClient;
    static List<string> msg = new List<string>();
    static Encrypt encrypt = new Encrypt();
    public static string currentVersion = "\"1.8.6\"";
    public static System.Guid clientId = System.Guid.NewGuid();

    public static JSONNode config;

    public static ProcessResponseDelegate OnLogin_Response;


    void readConfig()
    {
        FileStream fs = new FileStream(Application.streamingAssetsPath + "/config.json", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        NetClient.config = JSON.Parse(sr.ReadToEnd());
        NetClient.serverIP = JSON.GetStr(NetClient.config["network"]["serverIP"].ToString());
        NetClient.port = int.Parse(JSON.GetStr(NetClient.config["network"]["port"].ToString()));
    }


    public static void ProcessConnected(object sender, System.EventArgs args)
    {
        NetClient.isConnected = true;
        state = NetClientState.Connected;
    }

    public static void ProcessReceive(object sender, AsyncEventArgs args)
    {
        if (NetClient.isConnected)
        {
            var N = JSON.Parse(Encoding.UTF8.GetString(encrypt.Decode(args._sessions.RecvDataBuffer)));
            var versionString = N["version"].ToString();



            if (versionString != "" && currentVersion == versionString)
            {
                //SQL request
                var sqlStr = N["sql"].ToString();
                MsgType msgType = (MsgType)N["MsgType"].AsInt;
                switch (msgType)
                {
                    case MsgType.None:
                        break;
                    case MsgType.Message:
                        //SendString2All(N["WorldMsg"].ToString());
                        break;
                    case MsgType.Login:
                        //Login_Method(N, args);
                        break;
                    case MsgType.Login_Response:
                        if (OnLogin_Response != null)
                            OnLogin_Response.Invoke(N);
                        break;
                    case MsgType.Default:
                        break;
                    case MsgType.Transform:
                        break;
                    case MsgType.Sql:
                        break;
                    case MsgType.Regist:
                        break;
                    case MsgType.WorldMsg:
                        //SendString2All(N["WorldMsg"].ToString());
                        break;
                    default: break;
                }


            }
        }



        if (args._sessions.ClientSocket.Connected)
        {
            if (msg.Count >= 10)
                msg.Clear();
            var N = JSON.Parse(Encoding.UTF8.GetString(encrypt.Decode(args._sessions.RecvDataBuffer)));
            msg.Add(Regex.Unescape(JSON.GetStr(N["message"].ToString())) + "\n");
        }
    }

    public static void ProcessNetError(object sender, EventArgs args)
    {
        Application.LoadLevel(0);
    }

    public static JSONNode GetStandJson()
    {
        return JSON.Parse("{ \"version\": " + currentVersion + "}");
    }

    public static void SendJson(JSONNode N)
    {
        asyncClient.Send(encrypt.Encode(System.Text.Encoding.UTF8.GetBytes(N.ToString())));
    }

    

}

public enum NetClientState
{
    Loading=-1,
    Connected=0,
    Login=1,
    Home=2,
    RoomList=3,
    Room=4,
    Gameing=5,
    EndGame=6,
}

public delegate void VoidDelegate();
public delegate void ProcessResponseDelegate(JSONNode N);