using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class MoveSlotUI : MonoBehaviour
    {
		[SerializeField] private Button _button;
		[SerializeField] private Text _name;
		[SerializeField] private Text _pp;
		private Move _move;

		public Button Button => _button;
		public Move Move => _move;
		public event Action<MoveSlotUI> Submitted;

		public void SetMove(Move move)
		{
			_move = move;
			if (_move != null && _move.MoveData != null)
			{
				_name.text = _move.MoveData.Name;
				_pp.text = $"{_move.PP,2}/{_move.MoveData.MaxPP,2}";
			}
			else
			{
				_name.text = "-";
				_pp.text = "-";
			}
		}

        private void OnEnable()
        {
            _button.onClick.AddListener(() => Submitted?.Invoke(this));
        }

    }
}
