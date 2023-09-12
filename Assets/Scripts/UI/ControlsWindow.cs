using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ControlsWindow : Window
    {
        [SerializeField] private Button _closebutton;
        public event Action CloseClicked;

        private void OnEnable()
        {
            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        private void Start()
        {
            SetState(new ActiveState(this, _closebutton));
        }

    }
}
