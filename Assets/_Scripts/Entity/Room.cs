using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;


public class Room
{
    public int roomId;
    public string roomName = "";
    public string password = "";
    public Player owner;
    public List<Player> players;
    public bool isActive = false;
    public Room(int roomId)
    {
        this.roomId = roomId;
        isActive = false;

    }

    public Room(Player owner, List<Player> Players)
    {
        isActive = false;

    }

    public void JoinRoom(Player player)
    {
        players.Add(player);
        player.roomId = this.roomId;
        var M = NetClient.GetStandJson();
        M["MsgType"] = ((int)MsgType.Player_Join).ToString();
        M = player.ToJson(M);
        SendMessage(M);
    }




    public void SendMessage(string msg, Player sender)
    {
        var M = JSON.Parse("{ \"version\": " + NetClient.currentVersion + "}");
        M["MsgType"] = ((int)MsgType.RoomMsg).ToString();
        M["RoomMsg"] = msg;
        M["Sender"] = sender.clientId.ToString();
        SendMessage(M);
    }

    public void SendMessage(JSONNode json)
    {
        SendMessage(NetClient.encrypt.Encode(System.Text.Encoding.UTF8.GetBytes(json.ToString())));
    }

    private void SendMessage(byte[] data)
    {
        foreach (Player player in players)
        {
            NetClient.asyncClient.Send( data);
        }
    }





}


