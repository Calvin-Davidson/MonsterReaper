using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using SimpleJSON;
using UnityEngine;

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
        
        NetworkClient.Instance.Events.onTurnSwitch.AddListener(OnTurnSwitch);
        NetworkClient.Instance.Events.onConnected.AddListener(OnConnect);
        NetworkClient.Instance.Events.onGameStart.AddListener(OnGameStart);
    }

    public void OnPacketReceive(String message, NetworkData data)
    {
        Debug.Log(message);
        JSONNode jsonNode = JSONNode.Parse(message);
        string actionName = jsonNode["action"];

        NetworkEventContainer events = NetworkClient.Instance.Events;
        if (actionName == NetworkAction.Connected.ToString()) events.onConnected?.Invoke(jsonNode);
        if (actionName == NetworkAction.EndGame.ToString()) events.onGameEnd?.Invoke(jsonNode);
        if (actionName == NetworkAction.StartGame.ToString()) events.onGameStart?.Invoke(jsonNode);
        if (actionName == NetworkAction.TurnSwitch.ToString()) events.onTurnSwitch?.Invoke(jsonNode);
        if (actionName == NetworkAction.PlaceStone.ToString()) events.onStonePlace?.Invoke(jsonNode);
        if (actionName == NetworkAction.OpponentLeave.ToString()) events.onOpponentLeave?.Invoke(jsonNode);
        if (actionName == NetworkAction.GameFound.ToString()) events.onGameFound?.Invoke(jsonNode);
    }

    private void OnTurnSwitch(JSONNode node)
    {
        Debug.Log(node);
        NetworkClient.Instance.networkData.SetPlayerTurn(node["playerTurn"]);
    }

    private void OnConnect(JSONNode node)
    {
        NetworkClient.Instance.networkData.SetMyId(node["playerId"]);   
    }

    private void OnGameStart(JSONNode node)
    {
        Debug.Log(node);
        Debug.Log("starting player id: " + node["StartingPlayerId"]);
        NetworkClient.Instance.networkData.SetPlayerTurn(node["StartingPlayerId"]);
    }
}
