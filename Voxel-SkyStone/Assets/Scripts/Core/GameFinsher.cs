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
    public UnityEvent onGameDraw = new UnityEvent();
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

        VictoryState victoryState = skystoneGrid.GetWinner();
        if (victoryState == VictoryState.Loser) onGameLose?.Invoke();
        else if (victoryState == VictoryState.Winner) onGameWin?.Invoke();
        else onGameDraw?.Invoke();
        
        onGameEnd?.Invoke();
        
        FindObjectOfType<NetworkSendHandler>().SendGameEnd();
    }
}