using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public class RoomList : MonoBehaviour 
{
    public static RoomList Instance;
    public GameObject roomPrefab;
    public RectTransform grid;
    public List<Room> rooms;
    public List<GameObject> roomBtns;

    public GameObject CreateRoomPanel;

    public RoomListState state = RoomListState.None;

    public JSONNode responseData;
	// Use this for initialization
	void Start ()
    {
        Instance = this;
        GetRoomsFromService();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (state == RoomListState.GetResponse)
	    {
            StartCoroutine(RoomsResponse(responseData));
	    }
        else if (state == RoomListState.InRoom)
        {
            Application.LoadLevel(3);
        }
	}

    void OnEnable()
    {
        
    }
    /// <summary>
    /// 响应新建房间
    /// </summary>
    /// <param name="N"></param>
    public void CreateRoom_Response(JSONNode N)
    {
        state = RoomListState.InRoom;
        responseData = N;
    }

    /// <summary>
    /// 显示新建房间的面板
    /// </summary>
    public void btn_CreateRoom()
    {
        state = RoomListState.InputRoomInfo;
        CreateRoomPanel.SetActive(true);
        
    }
    /// <summary>
    /// 发送新建房间的请求，在新建面板的新建按钮
    /// </summary>
    public void CreateRoom()
    {
        if (CreateRoomPanel.GetComponent<RectTransform>().FindChild("roomName").GetComponent<InputField>().text != "")
        {
            JSONNode N = NetClient.GetStandJson();
            N["MsgType"] = ((int)MsgType.CreateRoom).ToString();
            N["clientId"] = NetClient.clientId.ToString();
            N["userId"] = NetClient.player.userId.ToString();
            /*room.roomName = N["roomName"].ToString();
                room.password = N["password"].ToString();*/
            N["roomName"] = CreateRoomPanel.GetComponent<RectTransform>().FindChild("roomName").GetComponent<InputField>().text;
            N["password"] = CreateRoomPanel.GetComponent<RectTransform>().FindChild("password").GetComponent<InputField>().text;
            Debug.Log(N.ToString());
            NetClient.SendJson(N);
        }
    }
    
    /// <summary>
    /// 取消新建房间的按钮
    /// </summary>
    public void CreateRoom_Cancel()
    {
        state = RoomListState.None;
        CreateRoomPanel.GetComponent<RectTransform>().FindChild("roomName").GetComponent<InputField>().text = "";
        CreateRoomPanel.GetComponent<RectTransform>().FindChild("password").GetComponent<InputField>().text = "";
        CreateRoomPanel.SetActive(false);
        foreach (var ss in roomBtns)
        {
            Destroy(ss.gameObject);
        }
        
        GetRoomsFromService();
        
    }

    /// <summary>
    /// 发送获取房间信息的请求
    /// </summary>
    public void GetRoomsFromService()
    {
        JSONNode N = NetClient.GetStandJson();
        N["MsgType"] = ((int)MsgType.RoomList).ToString();
        N["clientId"] = NetClient.clientId.ToString();
        N["userId"] = NetClient.player.userId.ToString();
        NetClient.SendJson(N);
        state = RoomListState.SendRequest;
    }
    /// <summary>
    /// 改变状态处理获取到的房间信息
    /// </summary>
    public void GetRoomsResponse(JSONNode N)
    {
        Debug.Log("get room list !");
        Debug.Log(N.ToString());
        responseData = N;
        state = RoomListState.GetResponse;
    }
    /// <summary>
    /// 获取所有的房间信息
    /// </summary>
    IEnumerator RoomsResponse(JSONNode N)
    {
        state = RoomListState.None;
        
        int count = N["RoomCount"].AsInt;
        //Debug.Log(count.ToString());
        for (int i = 0; i < count; i++)
        {
            GameObject room = (GameObject)UnityEngine.Object.Instantiate(roomPrefab);
            btnRoom btn = room.GetComponent<btnRoom>();
            btn.roomId = int.Parse(JSON.GetStr(N["Rooms"][i]["roomId"].ToString()));
            btn.roomName = JSON.GetStr(N["Rooms"][i]["roomName"]);
            btn.owner = JSON.GetStr(N["Rooms"][i]["owner"]);
            room.GetComponent<RectTransform>().parent = grid;
            room.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text = btn.roomId.ToString();
            room.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            roomBtns.Add(room);
            //yield return null;
        }
        yield return null;
    }

    public enum RoomListState
    {
        None = 0,
        SendRequest = 1,
        GetResponse = 2,
        InputRoomInfo = 3,
        InRoom = 4,
    }
}
