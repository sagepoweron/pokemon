using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/Inventory")]
    public class InventoryInfo : ScriptableObject
    {
        public int Gold;
        public List<ItemAmount> Medicines = new();
        public List<ItemAmount> Pokeballs = new();
    }
}
