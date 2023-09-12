using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/MoveData")]
    public class MoveData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _power;
        [SerializeField] private float _accuracy = 100;
        [SerializeField] private int _maxpp = 8;
        [SerializeField] private int _priority = 0;
        [SerializeField] private TypeData _elementtype;
        [SerializeField] private MoveCategory _category;
        [SerializeField] private List<MoveEffect> _usereffects;
        [SerializeField] private List<MoveEffect> _targeteffects;
        [SerializeField] private AudioClip _audioclip;

        public string Name => _name;
        public int Power => _power;
        public float Accuracy => _accuracy;
        public int MaxPP => _maxpp;
        public int Priority => _priority;
        public TypeData ElementType => _elementtype;
        public MoveCategory Category => _category;
        public List<MoveEffect> UserEffects => _usereffects;
        public List<MoveEffect> TargetEffects => _targeteffects;
        public AudioClip AudioClip => _audioclip;
    }
}
