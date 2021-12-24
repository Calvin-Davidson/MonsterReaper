using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class UIRaycastEvents : MonoBehaviour
{
    public UnityEvent MouseEnter = new UnityEvent();
    public UnityEvent MouseExit = new UnityEvent();
    public UnityEvent MouseClick = new UnityEvent();

    private bool _IsMouseOver = false;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;


    private void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        CheckMouse();
    }

    private void CheckMouse()
    {
        bool wasMouseOver = _IsMouseOver;
        
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        _IsMouseOver = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == this.gameObject)
            {
                _IsMouseOver = true;

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    MouseClick?.Invoke();
                }
                break;
            }
        }

        if (_IsMouseOver && !wasMouseOver) MouseEnter?.Invoke();
        if (!_IsMouseOver && wasMouseOver) MouseExit?.Invoke();
    }
}