using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame
{
    public class MoveSlot : MonoBehaviour, ISubmitHandler
    {
        [SerializeField] private Text _nametext;
        [SerializeField] private Text _pptext;
        private Move _move;
        public event Action<Move> MoveSubmitted;

        public void SetMove(Move move)
        {
            _move = move;
            if (_move != null && _move.MoveData != null)
            {
                _nametext.text = _move.MoveData.Name;
                _pptext.text = $"{_move.PP, 2}/{_move.MoveData.MaxPP, 2}";
            }
            else
            {
                _nametext.text = "-";
                _pptext.text = "-";
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            MoveSubmitted?.Invoke(_move);
        }
    }
}
