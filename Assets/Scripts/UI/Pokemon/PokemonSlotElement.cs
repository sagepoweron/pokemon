using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame
{
    public class PokemonSlotElement : MonoBehaviour, ISubmitHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _sprite;
        [SerializeField] private Text _name;
        [SerializeField] private Text _level;
        [SerializeField] private HealthBar _healthbarelement;
        private PokemonSlot _pokemonslot;

        public Button Button => _button;
        public PokemonSlot PokemonSlot => _pokemonslot;

        public event Action<PokemonSlotElement> Submitted;

        public void Initialize(PokemonSlot pokemonslot)
        {
            _pokemonslot = pokemonslot;
            _pokemonslot.Changed += UpdateImage;
            UpdateImage();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Submitted?.Invoke(this);
        }

        private void OnDisable()
        {
            if (_pokemonslot != null)
            {
                _pokemonslot.Changed -= UpdateImage;
            }
        }

        private void UpdateImage()
        {
            if (_pokemonslot != null && _pokemonslot.Pokemon != null && _pokemonslot.Pokemon.PokemonData != null)
            {
                _sprite.sprite = _pokemonslot.Pokemon.PokemonData.Sprite;
                _name.text = _pokemonslot.Pokemon.PokemonData.Name;
                _level.text = $"L{_pokemonslot.Pokemon.Level}";

                _healthbarelement.Initialize(_pokemonslot.Pokemon.MaxHP);
                _healthbarelement.Set(_pokemonslot.Pokemon.HP);

                _sprite.enabled = true;
                _name.enabled = true;
                _level.enabled = true;
                _healthbarelement.Show();
            }
            else
            {
                _sprite.enabled = false;
                _name.enabled = false;
                _level.enabled = false;
                _healthbarelement.Hide();
            }
        }
    }
}
