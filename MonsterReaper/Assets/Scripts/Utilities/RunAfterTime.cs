using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunAfterTime : MonoBehaviour
{
    [SerializeField] private float delay;

    public UnityEvent run = new UnityEvent();

    private float _runTime = 0f;
    private bool _isInvoked = false;
    private void Update()
    {
        _runTime += Time.deltaTime;

        if (_runTime >= delay && !_isInvoked)
        {
            _isInvoked = true;
            run?.Invoke();
        }
    }

    private void OnEnable()
    {
        _runTime = 0f;
        _isInvoked = false;
    }
}
