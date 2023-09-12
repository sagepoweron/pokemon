using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MovesPanel : MonoBehaviour
    {
        [SerializeField] private MoveSlot[] _slots;
        public event Action<Move> MoveSelected;

        private void Start()
        {
            //_slots = GetComponentsInChildren<MoveSlot>();
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].MoveSubmitted += OnMoveSelected;
            }
        }

        private void OnMoveSelected(Move move)
        {
            MoveSelected?.Invoke(move);
        }

        public void SetPokemon(Pokemon pokemoninstance)
        {
            ClearMoves();

            if (pokemoninstance == null || pokemoninstance.PokemonData == null)
            {
                return;
            }

            for (int i = 0; i < pokemoninstance.Moves.Count; i++)
            {
                _slots[i].SetMove(pokemoninstance.Moves[i]);
            }
        }

        public void ClearMoves()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].SetMove(null);
            }
        }
    }
}
