using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using SimpleJSON;
using UnityEngine;
using WebSocketSharp;

public class NetworkReceiveHandler : MonoBehaviour
{
    [SerializeField] private NetworkData networkData;
    private static NetworkReceiveHandler _instance;

    private void Start()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        _instance = this;
    }

    public void OnPacketReceive(MessageEventArgs args, NetworkData data)
    {
        Debug.Log(args.Data);
        JSONNode jsonNode = JSONNode.Parse(args.Data);
        string actionName = jsonNode["action"];

        NetworkEventContainer events = NetworkClient.Instance.Events;
        if (actionName == NetworkAction.Connected.ToString()) events.onConnected?.Invoke(jsonNode);
        if (actionName == NetworkAction.EndGame.ToString()) events.onGameEnd?.Invoke(jsonNode);
        if (actionName == NetworkAction.StartGame.ToString()) events.onGameStart?.Invoke(jsonNode);
        if (actionName == NetworkAction.TurnSwitch.ToString()) events.onTurnSwitch?.Invoke(jsonNode);
        if (actionName == NetworkAction.PlaceStone.ToString()) events.onStonePlace?.Invoke(jsonNode);
        if (actionName == NetworkAction.OpponentLeave.ToString()) events.onOpponentLeave?.Invoke(jsonNode);
    } 
}
