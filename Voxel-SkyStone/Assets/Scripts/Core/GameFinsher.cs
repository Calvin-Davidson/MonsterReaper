using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameFinsher : MonoBehaviour
{
    [SerializeField] private SkystoneGrid skystoneGrid;

    public UnityEvent onGameEnd = new UnityEvent();
    public UnityEvent onGameWin = new UnityEvent();
    public UnityEvent onGameLose = new UnityEvent();
    private void Start()
    {
        skystoneGrid.Stones.ForEach(stone =>
        {
            stone.onStonePlaced.AddListener(CheckGameEnd);
        });
    }

    private void CheckGameEnd()
    {
        if (!skystoneGrid.CheckGameEnd()) return;
        
        if (skystoneGrid.GetWinner() == NetworkClient.Instance.networkData.MyId) onGameWin?.Invoke();
        else onGameLose?.Invoke();
        
        onGameEnd?.Invoke();
        
        FindObjectOfType<NetworkSendHandler>().SendGameEnd();
    }
}