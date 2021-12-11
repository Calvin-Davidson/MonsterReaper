using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;

public class NetworkSendHandler : MonoBehaviour
{
    public void SendReady(string stone1Name, string stone2Name, string stone3Name, string stone4Name, string stone5Name)
    {
        Debug.Log("sending ready");
        var json = new
        {
            action = NetworkAction.Ready.ToString(),
            stone1 = stone1Name, 
            stone2 = stone2Name, 
            stone3 = stone3Name, 
            stone4 = stone4Name, 
            stone5 = stone5Name, 
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
}