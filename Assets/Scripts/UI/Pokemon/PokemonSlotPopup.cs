using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PokemonSlotPopup : MonoBehaviour
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Button _movebutton;
        [SerializeField] private Button _summarybutton;
        
        public event Action CloseButtonClicked;
        public event Action MoveButtonClicked;
        public event Action SummaryButtonClicked;

        private void OnEnable()
        {
            _closebutton.onClick.AddListener(() => CloseButtonClicked?.Invoke());
            _movebutton.onClick.AddListener(() => MoveButtonClicked?.Invoke());
            _summarybutton.onClick.AddListener(() => SummaryButtonClicked?.Invoke());

            _closebutton.Select();
        }
    }
}
