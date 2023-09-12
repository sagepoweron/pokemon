using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PokemonSummaryWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Image _sprite;
        [SerializeField] private Text _name;
        private Pokemon _pokemoninstance;
        [SerializeField] private MovesPanel _movespanel;
        [SerializeField] private RelearnMovesWindow _relearnmoveswindowprefab;

        public event Action CloseClicked;

        private void OnEnable()
        {
            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
            _closebutton.Select();
        }

        public void SetPokemon(Pokemon pokemoninstance)
        {
            _pokemoninstance = pokemoninstance;

            if (_pokemoninstance != null && _pokemoninstance.PokemonData != null)
            {
                _sprite.sprite = _pokemoninstance.PokemonData.Sprite;
                _name.text = _pokemoninstance.PokemonData.Name;

                //_healthbarcontroller.Fill(_pokemonslot.Pokemon.MaxHP, _pokemonslot.Pokemon.HP);

                _sprite.enabled = true;
                _name.enabled = true;
            }
            else
            {
                _sprite.enabled = false;
                _name.enabled = false;
                //_healthbarcontroller.Container.style.opacity = 0;
            }

            //if (Pokemon.IsValid(pokemon))
            //{
            //    _statspanel.SetPokemon(_pokemon);
            //    _movespanel.SetPokemon(_pokemon);

            //    _pokemon.LevelChanged += Refresh;
            //}
        }

        //private void OnDisable()
        //{
        //    if (Pokemon.IsValid(_pokemon))
        //    {
        //        _pokemon.LevelChanged -= Refresh;
        //    }
        //}

        //public void OpenRelearnMovesWindow()
        //{
        //    RelearnMovesWindow window = Instantiate(_relearnmoveswindowprefab);
        //    window.SetPokemon(_pokemon);
        //    window.CloseButton.clicked += () =>
        //    {
        //        Destroy(window.gameObject);
        //        Resume();
        //    };
        //    Pause(null);
        //}

        //private void Refresh()
        //{
        //    _statspanel.SetPokemon(_pokemon);
        //    _movespanel.SetPokemon(_pokemon);
        //}
    }
}
