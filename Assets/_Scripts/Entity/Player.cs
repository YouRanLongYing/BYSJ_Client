using UnityEngine;
using System.Collections;
using MyClient;
using SimpleJSON;

public class Player  {
    #region Feild
    private Session _session;
    public System.Guid clientId;
    public int roomId = -1;
    public int userId;
    public string nickName;
    public string userName;
    public bool isActive = false;
    #endregion


    public Session _Session
    {
        get { return _session; }
        set { _session = value; }
    }

    public Player() { }
    public Player(Session session)
    {


    }


    public JSONNode ToJson(JSONNode M)
    {
        M["clientId"] = clientId.ToString();
        M["userId"] = userId.ToString();
        M["userName"] = userName.ToString();
        M["nickName"] = nickName.ToString();
        return M;
    }
    public JSONNode ToJson(string parentNode, JSONNode M)
    {
        M[parentNode]["clientId"] = clientId.ToString();
        M[parentNode]["userId"] = userId.ToString();
        M[parentNode]["userName"] = userName.ToString();
        M[parentNode]["nickName"] = nickName.ToString();
        return M;
    }

    public void GetFromJson(string parentNode, JSONNode M)
    {
        this.clientId = NetClient.clientId;
        //this.clientId = Guid.Parse(JSON.GetStr(M[parentNode]["clientId"]));
        userId = int.Parse(JSON.GetStr(M[parentNode]["userId"]).ToString());
        userName = JSON.GetStr(M[parentNode]["userName"]).ToString();
        nickName = JSON.GetStr(M[parentNode]["nickName"]).ToString();
    }
            

}
