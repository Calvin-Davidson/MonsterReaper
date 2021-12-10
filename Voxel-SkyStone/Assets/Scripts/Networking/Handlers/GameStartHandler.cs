using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Networking;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartHandler : MonoBehaviour
{
    private void Start()
    {
        NetworkClient.Instance.Events.onGameStart.AddListener(StartGame);
    }
    
    private void OnDisable()
    {
        NetworkClient.Instance.Events.onGameStart.RemoveListener(StartGame);
    }

    private void StartGame(JSONNode node)
    {
        Debug.Log("starting game scene");
        SceneManager.LoadScene(1);
        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        Debug.Log("yest");
        yield return null;
    }
}