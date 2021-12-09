using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stone", order = 1)]
    public class TileScriptableObject : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private bool isWall;
        [SerializeField] private GameObject image;
        [SerializeField] private int topDamage;
        [SerializeField] private int rightDamage;
        [SerializeField] private int bottomDamage;
        [SerializeField] private int leftDamage;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool IsWall
        {
            get => isWall;
            set => isWall = value;
        }

        public GameObject Image
        {
            get => image;
            set => image = value;
        }

        public int TopDamage => topDamage;

        public int RightDamage => rightDamage;

        public int BottomDamage => bottomDamage;

        public int LeftDamage => leftDamage;
    }
}
