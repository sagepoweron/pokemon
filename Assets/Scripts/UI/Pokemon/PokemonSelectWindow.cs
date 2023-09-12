using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PokemonSelectWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Transform _pokemonslotparent;
        [SerializeField] private PokemonSlotElement _pokemonslotelementprefab;

        public event Action CloseClicked;
        public event Action<PokemonSlot> PokemonSlotSelected;

        private void OnEnable()
        {
            CreateSlots();

            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        private void Start()
        {
            SetState(new ActiveState(this, _closebutton));
        }

        private void CreateSlots()
        {
            for (int i = 0; i < PartyManager.Instance.Slots.Length; i++)
            {
                PokemonSlotElement pokemonslotelement = Instantiate(_pokemonslotelementprefab, _pokemonslotparent);
                pokemonslotelement.Initialize(PartyManager.Instance.Slots[i]);
                pokemonslotelement.Submitted += PokemonSlotElementSubmitted;
            }
        }





        private void PokemonSlotElementSubmitted(PokemonSlotElement pokemonslotelement)
        {
            PokemonSlotSelected?.Invoke(pokemonslotelement.PokemonSlot);
        }


    }
}
