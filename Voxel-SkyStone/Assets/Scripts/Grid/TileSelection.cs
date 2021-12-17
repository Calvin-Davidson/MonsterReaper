using System;
using Grid;
using UnityEngine;
using UnityEngine.Events;

public class TileSelection : MonoBehaviour
{
    [SerializeField] private StoneData selectedStone;

    public UnityEvent<String> onStonePlace = new UnityEvent<String>();
    
    private SkystoneGrid _skyStoneGrid;

    private void Awake()
    {
        _skyStoneGrid = FindObjectOfType<SkystoneGrid>();
    }

    private void Update()
    {
        if (!NetworkClient.Instance.networkData.IsMyTurn()) return;
        if (selectedStone == null) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.gameObject.TryGetComponent<Stone>(out Stone clickedStone);
                if (clickedStone == null) return;
                if (clickedStone.StoneData != null) return;

                clickedStone.TeamSide = NetworkClient.Instance.networkData.MyId;
                clickedStone.StoneData = selectedStone;
                onStonePlace?.Invoke(selectedStone.Name);
                FindObjectOfType<NetworkSendHandler>().PlaceStone(selectedStone.Name, _skyStoneGrid.Stones.IndexOf(clickedStone));
                selectedStone = null;
            }
        }
    }

    public StoneData SelectedStone
    {
        get => selectedStone;
        set => selectedStone = value;
    }
}
