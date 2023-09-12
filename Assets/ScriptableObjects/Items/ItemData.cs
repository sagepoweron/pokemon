using UnityEngine;

namespace MyGame
{
    //[CreateAssetMenu(menuName = "ScriptableObject/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description = "No description.";
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _cost = 100;
        [SerializeField] private int _maxamount = 9;
        
        public string Name => _name;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public int Cost => _cost;
        public int MaxAmount => _maxamount;
    }
}