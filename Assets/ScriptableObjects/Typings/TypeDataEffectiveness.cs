using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class TypeDataEffectiveness
    {
        [SerializeField] private TypeData _typedata;
        [SerializeField] private float _effectiveness;

        public TypeData TypeData => _typedata;
        public float Effectiveness => _effectiveness;
    }
}
