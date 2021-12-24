using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

public class TurnHandler : MonoBehaviour
{

    public UnityEvent onTurnEnd = new UnityEvent();
    public UnityEvent onTurnStart = new UnityEvent();

    private bool _started = false;
    
    public void OnTurnChange(JSONNode data)
    {
        if (NetworkClient.Instance.networkData.IsMyTurn() && !_started)
        {
            onTurnStart?.Invoke();
            _started = true;
        } 
        else if (!NetworkClient.Instance.networkData.IsMyTurn() && _started)
        {
            onTurnEnd?.Invoke();
            _started = false;
        }
    }
}
