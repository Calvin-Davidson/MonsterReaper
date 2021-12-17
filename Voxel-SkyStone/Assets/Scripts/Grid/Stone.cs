using UnityEngine;
using UnityEngine.Events;

namespace Grid
{
    public class Stone : MonoBehaviour
    {
        [SerializeField] private int teamSide;
        [SerializeField] private StoneData stoneData;
        [SerializeField] private GameObject tileObjectContainer;
        [SerializeField] private MeshRenderer tileOutline;

        [SerializeField] private GameObject topArrowContainer;
        [SerializeField] private GameObject rightArrowContainer;
        [SerializeField] private GameObject bottomArrowContainer;
        [SerializeField] private GameObject leftArrowContainer;

        [SerializeField] private GameObject[] arrows;

        public UnityEvent onStoneDataUpdate = new UnityEvent();
        public UnityEvent onStonePlace = new UnityEvent();
        public UnityEvent onStonePlaced = new UnityEvent();
        public UnityEvent onTeamChange = new UnityEvent();

        private int _gridIndex;

        private void UpdateStoneData()
        {
            tileObjectContainer.GetComponent<MeshRenderer>().material.mainTexture = stoneData.Texture;
            onStoneDataUpdate?.Invoke();
            
            SpawnArrows();
        }

        private void RenderTileOutlineColor()
        {
            tileOutline.material.color = teamSide == NetworkClient.Instance.networkData.MyId ? Color.green : Color.red;
        }
        
        private void SpawnArrows()
        {
            if (stoneData.TopDamage > 0) SpawnArrow(topArrowContainer, arrows[stoneData.TopDamage-1]);
            if (stoneData.RightDamage > 0) SpawnArrow(rightArrowContainer, arrows[stoneData.RightDamage-1]);
            if (stoneData.BottomDamage > 0) SpawnArrow(bottomArrowContainer, arrows[stoneData.BottomDamage-1]);
            if (stoneData.LeftDamage > 0) SpawnArrow(leftArrowContainer, arrows[stoneData.LeftDamage-1]);
        }

        private void SpawnArrow(GameObject container, GameObject arrow)
        {
            Instantiate(arrow, container.transform, false);
        }

        public int GridIndex
        {
            get => _gridIndex;
            set => _gridIndex = value;
        }

        public int TeamSide
        {
            get => teamSide;
            set
            {
                teamSide = value;
                RenderTileOutlineColor();
                onTeamChange?.Invoke();
            }
        }

        public StoneData StoneData
        {
            get => stoneData;
            set
            {
                stoneData = value;
                if (value != null) onStonePlace?.Invoke();
                UpdateStoneData();
                
                onStonePlaced?.Invoke();
            }
        }
    }
}