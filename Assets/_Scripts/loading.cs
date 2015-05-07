using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System.Text;
using MyClient;
using System.Net;
using System.Net.Sockets;

public class loading : MonoBehaviour {
	private static bool isConnected=false;
	private bool isClick=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isConnected)
		{
			Application.LoadLevel(1);
		}
	}

	void OnGUI()
	{
		if(isClick==false)
		{
			Test.serverIP= GUI.TextField(new Rect(0f,0f,Screen.width,Screen.height*0.5f),Test.serverIP);
			if(GUI.Button(new Rect(0f,Screen.height*0.5f,Screen.width,Screen.height*0.5f),"连接"))
			{
				//Debug.Log("Connecting....");
				IPAddress ep=IPAddress.Parse(Test.serverIP);
				//IPAddress ep=IPAddress.Parse("127.0.0.1");
				Test.ac=new AsyncClient(new IPAddress[]{ ep},8421,null);
				Test.ac.DatagramReceived+=Test.ProcessReceive;
				Test.ac.ServerConnected+=ProcessConnected;
				Test.ac.Connect();
			}
		}
		else
		{
			GUI.Label(new Rect(Screen.width*0.25f,Screen.height*0.25f,Screen.width*0.5f,Screen.height*0.5f),"Connecting to "+Test.serverIP);

		}
	}

	static void ProcessConnected(object sender,System.EventArgs args)
	{
		isConnected=true;
	}



}
