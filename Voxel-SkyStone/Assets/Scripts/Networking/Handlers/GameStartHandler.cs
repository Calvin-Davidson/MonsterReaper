using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartHandler : MonoBehaviour
{
    [SerializeField] private NetworkData networkData;

    private void Awake()
    {
        networkData.networkEvents.onGameStart.AddListener(StartGame);
    }

    private void OnDisable()
    {
        networkData.networkEvents.onGameStart.RemoveListener(StartGame);
    }

    private void StartGame(JSONNode node)
    {
        GetComponent<SceneSwitcher>().LoadScene();
    }
}
