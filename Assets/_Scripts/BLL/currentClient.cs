using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;
using MyClient;
using System.Text;
using System.Text.RegularExpressions;

public class currentClient  {

    public static string serverIP = "127.0.0.1";

    static string the_JSON_string = "";
    string cmd = "";
    static string msg = "";
    static System.Guid guid = System.Guid.NewGuid();
    static Encrypt encrypt = new Encrypt();
    //public static AsyncClient ac;




    public static void ProcessReceive(object sender, AsyncEventArgs args)
    {
        if (args._sessions.ClientSocket.Connected)
        {
            if (msg.Length >= 512)
                msg = "";
            //msg+="source: "+Encoding.UTF8.GetString(encrypt.Decode( args._sessions.RecvDataBuffer))+"\n";
            var N = JSON.Parse(Encoding.UTF8.GetString(encrypt.Decode(args._sessions.RecvDataBuffer)));
            //msg+="Received from server: \n"+Regex.Unescape( JSON.GetStr( N["message"].ToString()))+"\n";
            msg += Regex.Unescape(JSON.GetStr(N["message"].ToString())) + "\n"; 
        }
    }

    static void OpenFile(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sw = new StreamReader(fs, Encoding.UTF8);
        the_JSON_string = sw.ReadToEnd();
        sw.Close();
        fs.Close();
    }

}
