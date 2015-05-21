using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using MyClient;
using SimpleJSON;
using UnityEngine;

class MainClass : MonoBehaviour 
{
	static string cmd="";
	public static void Main (string[] args)
	{
		Console.WriteLine ("Hello World!");
//			IPAddress ep=IPAddress.Parse("127.0.0.1");
		IPAddress ep=IPAddress.Parse("127.0.0.1");
		AsyncClient ac=new AsyncClient(new IPAddress[]{ ep},8421,null);

		Console.WriteLine(ac.Connected.ToString()); 
		ac.DatagramReceived+=ProcessReceive;
		//ac.Connect();
		Console.WriteLine(ac.Connected.ToString());
		ac.Send(System.Text.Encoding.UTF8.GetBytes( "Hello service!"));
		Console.WriteLine(ac.Connected.ToString());



		while(true)
		{
			cmd=Console.ReadLine();
			if(cmd=="stop")
			{
				break;
			}
			else
			{
				ac.Send(System.Text.Encoding.UTF8.GetBytes( cmd));
			}
		}
	}


	static void ProcessReceive(object sender,AsyncEventArgs args)
	{
		Console.WriteLine("Received from server: "+Encoding.UTF8.GetString( args._sessions.RecvDataBuffer));
	}

}

