using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/Medicine")]
    public class Medicine : ItemData
    {
        [SerializeField] private int _recoveramount = 20;
        [SerializeField] private bool _revive;
        
        //[field: SerializeField] public int HealAmount { get; private set; } = 20;

        public int RecoverAmount => _recoveramount;
        public bool Revive => _revive;
    }
}
