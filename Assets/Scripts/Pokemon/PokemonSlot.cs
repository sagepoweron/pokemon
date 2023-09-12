using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class PokemonSlot
    {
        [SerializeField] private Pokemon _pokemon;
        public Pokemon Pokemon
        {
            get => _pokemon;
            set
            {
                if (_pokemon != null)
                {
                    _pokemon.LevelChanged -= OnPokemonChanged;
                }
                _pokemon = value;
                if (_pokemon != null)
                {
                    _pokemon.LevelChanged += OnPokemonChanged;
                }

                Changed?.Invoke();
            }
        }
        public event Action Changed;

        private void OnPokemonChanged()
        {
            Changed?.Invoke();
        }
    }
}
