using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System.Text;
using MyClient;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
//using System;

public class Test : MonoBehaviour {
	public GUIStyle gs;

	public static string serverIP="192.168.111.125";

	string the_JSON_string="";
	string cmd="";
	static string msg="";
	static System.Guid guid=System.Guid.NewGuid();
	static Encrypt encrypt=new Encrypt();
	public static AsyncClient ac;

	public JSONNode jsonData=JSON.Parse("{ \"version\": \"1.8.6\"}");
	// Use this for initialization
	void Start () {

		



//		msg+=ac.Connected.ToString()+"\n";
//		string hello="{ \"version\": \"1.8.6\"}";
//		ac.Send(encrypt.Encode( System.Text.Encoding.UTF8.GetBytes( hello)));
//
//		msg+=ac.Connected.ToString()+"\n";
		//StartCoroutine(isConnect());
			
			


		
		


	}

	public static void ProcessReceive(object sender,AsyncEventArgs args)
	{
		if(args._sessions.ClientSocket.Connected)
		{
			if(msg.Length>=512)
				msg="";
			//msg+="source: "+Encoding.UTF8.GetString(encrypt.Decode( args._sessions.RecvDataBuffer))+"\n";
			var N=JSON.Parse(Encoding.UTF8.GetString(encrypt.Decode( args._sessions.RecvDataBuffer)));
			//msg+="Received from server: \n"+Regex.Unescape( JSON.GetStr( N["message"].ToString()))+"\n";
			msg+=Regex.Unescape( JSON.GetStr( N["message"].ToString()))+"\n";
		}
	}
	

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D))
		{
			Debug.Log( ac.Connected.ToString());
		}
	}

	void OnGUI()
	{
		cmd= GUILayout.TextArea(cmd);
		if(GUILayout.Button("Send"))
		{
			if(cmd=="stop")
			{
				return;
			}
			else
			{
				jsonData["message"]=cmd;
				ac.Send(encrypt.Encode( System.Text.Encoding.UTF8.GetBytes( jsonData.ToString())));
				cmd="";
			}
		}
		GUILayout.Label(ac.Connected.ToString(),gs);
		GUILayout.Label(msg,gs);


	}


	IEnumerator isConnect()
	{
		while(ac.Connected==false)
		{
			//ac.Connect();

			yield return new WaitForSeconds(0.5f);
		}
		string hello="{ \"version\": \"1.8.6\"}";
		ac.Send(encrypt.Encode( System.Text.Encoding.UTF8.GetBytes( hello)));
		yield return null;
	}


	void OpenFile()
	{
		print(Application.streamingAssetsPath+"/jsonTest.txt");
		OpenFile(Application.streamingAssetsPath+"/jsonTest.txt");
	}

	void OpenFile(string path)
	{
		FileStream fs=new  FileStream(path,FileMode.Open);
		StreamReader sw=new StreamReader(fs,Encoding.UTF8);
		the_JSON_string=sw.ReadToEnd();
		sw.Close();
		fs.Close();
	}

	/// <summary>
	/// {
	///	"version": "1.0",
	///	"data": {
	///		"sampleArray": [
	///		    "string value",
	///		    5,
	///		    {
	///			"name": "sub object"
	///		    }
	///		    ]
	///	}
	///}
	/// </summary>
	void MyTest()
	{
		var N = JSON.Parse(the_JSON_string);
		var versionString = N["version"].Value;        // versionString will be a string containing "1.0"
		var versionNumber = N["version"].AsFloat;      // versionNumber will be a float containing 1.0
		var name = N["data"]["sampleArray"][2]["name"];// name will be a string containing "sub object"
		
		//C#
		string val = N["data"]["sampleArray"][0];      // val contains "string value"
		
		//UnityScript
		//var val : String = N["data"]["sampleArray"][0];// val contains "string value"
		
		var i = N["data"]["sampleArray"][1].AsInt;     // i will be an integer containing 5
		N["data"]["sampleArray"][1].AsInt = i+6;       // the second value in sampleArray will contain "11"

		N["additional"]["second"]["name"] = "FooBar";  // this will create a new object named "additional" in this object create another
		//object "second" in this object add a string variable "name"


		var mCount = N["countries"]["germany"]["moronCount"].AsInt; // this will return 0 and create all the required objects and
		// initialize "moronCount" with 0.
		
		if (N["wrong"] != null)                        // this won't execute the if-statement since "wrong" doesn't exist
		{}
		if (N["wrong"].AsInt == 0)                     // this will execute the if-statement and in addition add the "wrong" value.
		{}
		
		N["data"]["sampleArray"][-1] = "Test";         // this will add another string to the end of the array
		N["data"]["sampleArray"][-1]["name"] = "FooBar"; // this will add another object to the end of the array which contains a string named "name"

		//擦除数据
		N["data"] = "erased";       


		print(N.SaveToBase64());
		N.SaveToFile(Application.streamingAssetsPath+"/JsonData.txt");
	}
}
