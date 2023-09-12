using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class MoveSelectWindow : Window
	{
		[SerializeField] private Button _closebutton;
		[SerializeField] private Transform _moveslotparent;
		private MoveSlotUI[] _moveslotelements;
		public event Action CloseClicked;
		public event Action<Move> MoveSubmitted;

		private void OnEnable()
		{
			_moveslotelements = _moveslotparent.GetComponentsInChildren<MoveSlotUI>();

			_closebutton.onClick.AddListener(() => CloseClicked?.Invoke());

			for (int i = 0; i < _moveslotelements.Length; i++)
			{
				_moveslotelements[i].Submitted += OnMoveSubmitted;
			}

			_closebutton.Select();
		}

		private void OnMoveSubmitted(MoveSlotUI move)
		{
			MoveSubmitted?.Invoke(move.Move);
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
				_moveslotelements[i].SetMove(pokemoninstance.Moves[i]);
			}
        }

		public void ClearMoves()
		{
			for (int i = 0; i < _moveslotelements.Length; i++)
			{
				_moveslotelements[i].SetMove(null);
			}
		}

	}
}
