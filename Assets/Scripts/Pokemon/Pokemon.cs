using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class Pokemon
    {
        [SerializeField] private PokemonData _pokemondata;
        [SerializeField] private int _level;
        private int _maxhp;
        private int _hp;
        private int _attack;
        private int _defense;
        private int _specialattack;
        private int _specialdefense;
        private int _speed;
        private readonly List<MoveData> _learnedmoves = new();
        private readonly List<Move> _moves = new();
        //private StatusEffect _statuseffect = StatusEffect.normal;
        public event Action Fainted;
        public event Action LevelChanged;


        public PokemonData PokemonData => _pokemondata;
        public int Level => _level;
        public int MaxHP => _maxhp;
        public int HP
        {
            get => _hp;
            set
            {
                if (_hp > 0 && value <= 0)
                {
                    Fainted?.Invoke();
                }
                _hp = Mathf.Clamp(value, 0, _maxhp);
            }
        }
        public int Attack => _attack;
        public int Defense => _defense;
        public int SpecialAttack => _specialattack;
        public int SpecialDefense => _specialdefense;
        public int Speed => _speed;
        public List<MoveData> LearnedMoves => _learnedmoves;
        public List<Move> Moves => _moves;

        public Pokemon(PokemonData info, int level)
        {
            if (info == null)
            {
                return;
            }
            _pokemondata = info;
            SetLevel(level);
            InitializeMoves();
        }

        public Pokemon(Pokemon pokemon)
        {
            if (pokemon == null || pokemon._pokemondata == null)
            {
                return;
            }
            _pokemondata = pokemon._pokemondata;
            SetLevel(pokemon._level);
            InitializeMoves();
        }

        public void SetLevel(int level)
        {
            _level = Mathf.Clamp(level, 1, 100);
            _maxhp = Mathf.FloorToInt(2 * _pokemondata.MaxHP * _level / 100) + _level + 10;
            _hp = _maxhp;
            _attack = Mathf.FloorToInt(2 * _pokemondata.Attack * _level / 100) + 5;
            _defense = Mathf.FloorToInt(2 * _pokemondata.Defense * _level / 100) + 5;
            _specialattack = Mathf.FloorToInt(2 * _pokemondata.SpecialAttack * _level / 100) + 5;
            _specialdefense = Mathf.FloorToInt(2 * _pokemondata.SpecialDefense * _level / 100) + 5;
            _speed = Mathf.FloorToInt(2 * _pokemondata.Speed * _level / 100) + 5;

            for (int i = 0; i < _pokemondata.LevelUpMoves.Count; i++)
            {
                if (_level >= _pokemondata.LevelUpMoves[i].Level)
                {
                    MoveData movedata = _pokemondata.LevelUpMoves[i].MoveData;
                    if (!_learnedmoves.Contains(movedata))
                    {
                        _learnedmoves.Add(movedata);
                    }

                    if (!HasMove(movedata))
                    {
                        if (_moves.Count < 4)
                        {
                            _moves.Add(new Move(movedata));
                        }
                    }
                }
            }

            LevelChanged?.Invoke();
        }


        private void InitializeMoves()
        {
            for (int i = 0; i < _moves.Count && i < _learnedmoves.Count; i++)
            {
                if (_moves[i] == null || _moves[i].MoveData == null)
                {
                    _moves[i] = new Move(_learnedmoves[i]);
                }
            }
        }

        public bool HasMove(MoveData moveinfo)
        {
            for (int i = 0; i < _moves.Count; i++)
            {
                if (_moves[i] != null && _moves[i].MoveData == moveinfo)
                {
                    return true;
                }
            }

            return false;
        }


        public void Restore()
        {
            _hp = _maxhp;

            for (int i = 0; i < _moves.Count; i++)
            {
                _moves[i].Restore();
            }
        }

        public static bool CanBattle(Pokemon pokemon)
        {
            return pokemon != null && pokemon.PokemonData != null && pokemon._hp > 0;
        }
    }
}
