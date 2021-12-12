using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;

public class NetworkSendHandler : MonoBehaviour
{
    public void SendReady(string[] stones)
    {
        Debug.Log("sending ready");
        var json = new
        {
            action = NetworkAction.Ready.ToString(),
            stones
        };
        NetworkClient.Instance.Send(Newtonsoft.Json.JsonConvert.SerializeObject(json));
    }

    public void SendUnready()
    {
        Debug.Log("sending unready");
        var json = new
        {
            action = NetworkAction.Unready.ToString(),
        };
        NetworkClient.Instance.Send(Newtonsoft.Json.JsonConvert.SerializeObject(json));
    }

    public void PlaceStone(string stoneName, int gridId)
    {
        var json = new
        {
            action = NetworkAction.PlaceStone.ToString(),
            stoneName,
            gridId
        };
        NetworkClient.Instance.Send(Newtonsoft.Json.JsonConvert.SerializeObject(json));
    }

    public void SendGameEnd()
    {
        var json = new
        {
            action = NetworkAction.EndGame.ToString()
        };
        NetworkClient.Instance.Send(Newtonsoft.Json.JsonConvert.SerializeObject(json));
    }
}