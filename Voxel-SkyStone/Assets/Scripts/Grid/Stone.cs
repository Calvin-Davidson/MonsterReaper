using UnityEngine;

namespace Grid
{
    public class Stone : MonoBehaviour
    {
        [SerializeField] private int teamSide;
        [SerializeField] private StoneData stoneData;

        private Vector3 _imageOffset = new Vector3(0, 0.01f, 0);
        private int _gridIndex;

        public void UpdateStoneData()
        {
            GameObject tileImage = Instantiate(stoneData.Image, transform.position + _imageOffset, Quaternion.identity);
            tileImage.transform.parent = transform;
            name = stoneData.name;
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
