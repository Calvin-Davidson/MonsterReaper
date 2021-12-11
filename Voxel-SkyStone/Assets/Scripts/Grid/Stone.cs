using UnityEngine;
using UnityEngine.Events;

namespace Grid
{
    public class Stone : MonoBehaviour
    {
        [SerializeField] private int teamSide;
        [SerializeField] private StoneData stoneData;
        [SerializeField] private GameObject tileObjectContainer;

        public UnityEvent onStoneDataUpdate = new UnityEvent();
        
        private Vector3 _imageOffset = new Vector3(0, 0.01f, 0);
        private int _gridIndex;

        public void UpdateStoneData()
        {
            if (stoneData.TileObject != null)
            {
               GameObject tileImage = Instantiate(stoneData.TileObject, transform.position + _imageOffset, Quaternion.identity);
               tileImage.transform.parent = tileObjectContainer.transform;
            }
            onStoneDataUpdate?.Invoke();
        }

        public int GridIndex
        {
            get => _gridIndex;
            set => _gridIndex = value;
        }

        public int TeamSide
        {
            get => teamSide;
            set => teamSide = value;
        }
        public StoneData StoneData
        {
            get => stoneData;
            set => stoneData = value;
        }
    }
}
