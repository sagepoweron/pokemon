using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PauseWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Button _pokemonbutton;
        [SerializeField] private Button _itemsbutton;
        [SerializeField] private ManagePokemonWindow _pokemonwindowprefab;
        [SerializeField] private ManageItemsWindow _itemswindowprefab;
        public event Action CloseClicked;

        private void OnEnable()
        {
            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
            _itemsbutton.onClick.AddListener(() => SetState(new ManageItemsState(this)));
            _pokemonbutton.onClick.AddListener(() => SetState(new ManagePokemonState(this)));
        }

        private void Start()
        {
            SetState(new ActiveState(this, _closebutton));
        }

        private class ManagePokemonState : State
        {
            private readonly PauseWindow _owner;
            private ManagePokemonWindow _managepokemonwindow;
            public ManagePokemonState(PauseWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _managepokemonwindow = Instantiate(_owner._pokemonwindowprefab);
                _managepokemonwindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _owner._pokemonbutton));
            }

            public override void End()
            {
                _managepokemonwindow.CloseWindow();
            }
        }
        private class ManageItemsState : State
        {
            private readonly PauseWindow _owner;
            private ManageItemsWindow _manageitemswindow;
            public ManageItemsState(PauseWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _manageitemswindow = Instantiate(_owner._itemswindowprefab);
                _manageitemswindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _owner._itemsbutton));
            }

            public override void End()
            {
                _manageitemswindow.CloseWindow();
            }
        }

        
    }
}
