using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseEvents : MonoBehaviour
{
    public UnityEvent onMouseEnter;
    public UnityEvent onMouseExit;
    public UnityEvent onMouseClick;

    private bool _isMouseOver = false;

    private void OnMouseDown()
    {
        onMouseClick?.Invoke();
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == this.gameObject && !_isMouseOver)
            {
                _isMouseOver = true;
                onMouseEnter?.Invoke();
            }

            if (hit.collider.gameObject != this.gameObject && _isMouseOver)
            {
                _isMouseOver = false;
                onMouseExit?.Invoke();
            }
        }
        else if (_isMouseOver)
        {
            _isMouseOver = false;
            onMouseExit?.Invoke();
        }
    }
}
