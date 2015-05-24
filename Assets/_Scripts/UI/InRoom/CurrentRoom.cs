using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class CurrentRoom : MonoBehaviour {
    public static CurrentRoom Instance;

    public Room room;
    public Text roomName;
    public Text ownerText;

    

	// Use this for initialization
	void Start () 
    {
        Instance = this;

        JSONNode N = RoomList.Instance.responseData;
        Debug.Log(N.ToString());
        roomName.text = JSON.GetStr(N["roomName"].ToString());
        ownerText.text ="房主: " +JSON.GetStr(N["ownerName"].ToString());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        Application.LoadLevel(4);
    }

    public void GetPlayers()
    {
        JSONNode N = NetClient.GetStandJson();
        N["MsgType"] = ((int)MsgType.Room).ToString();
        N["clientId"] = NetClient.clientId.ToString();
        N["userId"] = NetClient.player.userId.ToString();
        N["roomId"] = room.roomId.ToString();
        
    }


}
