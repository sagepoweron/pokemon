using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/TypeData")]
    public class TypeData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<TypeDataEffectiveness> _typedataeffectivenesslist = new();

        public string Name => _name;
        public List<TypeDataEffectiveness> TypeDataEffectivenessList => _typedataeffectivenesslist;
    }
}
