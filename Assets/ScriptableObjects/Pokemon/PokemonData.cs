using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/PokemonData")]
    public class PokemonData : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] Sprite _sprite;
        [SerializeField] Sprite _backsprite;
        [SerializeField] int _maxhp = 10;
        [SerializeField] int _attack = 1;
        [SerializeField] int _defense = 1;
        [SerializeField] int _specialattack = 1;
        [SerializeField] int _specialdefense = 1;
        [SerializeField] int _speed = 1;
        [SerializeField] int _capturerate = 45;
        [SerializeField] TypeData _type1;
        [SerializeField] TypeData _type2;
        [SerializeField] List<MoveData> _allpossiblemoves;
        [SerializeField] List<MoveDataLevel> _levelupmoves;
        [SerializeField] AudioClip _cry;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public Sprite BackSprite => _backsprite;
        public int MaxHP => _maxhp;
        public int Attack => _attack;
        public int Defense => _defense;
        public int SpecialAttack => _specialattack;
        public int SpecialDefense => _specialdefense;
        public int Speed => _speed;
        public int Capturerate => _capturerate;
        public TypeData Type1 => _type1;
        public TypeData Type2 => _type2;
        public List<MoveDataLevel> LevelUpMoves => _levelupmoves;
        public AudioClip Cry => _cry;
    }
}
