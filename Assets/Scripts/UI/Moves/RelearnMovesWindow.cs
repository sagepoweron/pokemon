using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame
{
    public class RelearnMovesWindow : Window
    {
        private readonly Button _closebutton;
        [SerializeField] private MoveSlot _moveslotprefab;
        [SerializeField] private Transform _moveslotparent;
        //private Pokemon _pokemon;

        public Button CloseButton => _closebutton;

        public void SetPokemon(Pokemon pokemoninstance)
        {
            //_pokemon = pokemon;
            if (pokemoninstance == null || pokemoninstance.PokemonData == null)
            {
                return;
            }

            for (int i = 0; i < pokemoninstance.LearnedMoves.Count; i++)
            {
                MoveSlot moveslot = Instantiate(_moveslotprefab, _moveslotparent);
                moveslot.SetMove(new (pokemoninstance.LearnedMoves[i]));
            }
        }

    }
}
