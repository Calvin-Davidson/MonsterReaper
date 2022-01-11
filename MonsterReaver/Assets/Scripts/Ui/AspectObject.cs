using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AspectObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float targetAspect;
    [SerializeField] private float tolerance;
    
    [SerializeField] private UnityEvent validAspectChange;
    [SerializeField] private UnityEvent invalidAspectChange;
    
    private bool _wasValid = false;

    private void Awake()
    {
        float aspect = (float) Screen.width / Screen.height;
        if (Math.Abs(targetAspect - aspect) < tolerance)
        {
            if (targetObject != null) targetObject.SetActive(true);
            validAspectChange?.Invoke();
            _wasValid = true;
        }
        else
        {
            if (targetObject != null) targetObject.SetActive(false);
            invalidAspectChange?.Invoke();
            _wasValid = false;
        }
    }

    private void Update()
    {
        float aspect = (float) Screen.width / Screen.height;

        Debug.Log(aspect);
        if (Math.Abs(targetAspect - aspect) < tolerance && !_wasValid)
        {
            if (targetObject != null) targetObject.SetActive(true);
            validAspectChange?.Invoke();
            _wasValid = true;
        }
        else if (_wasValid && Math.Abs(targetAspect - aspect) > tolerance)
        {
            if (targetObject != null) targetObject.SetActive(false);
            invalidAspectChange?.Invoke();
            _wasValid = false;
        }
    }
}
