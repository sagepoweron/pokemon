using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace MyGame
{
    public class ManagePokemonWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Transform _pokemonslotparent;
        [SerializeField] private PokemonSlotElement _pokemonslotelementprefab;
        [SerializeField] private PokemonSlotPopup _popupprefab;
        [SerializeField] private PokemonSummaryWindow _summarywindowprefab;

        public event Action CloseClicked;

        private void OnEnable()
        {
            CreateSlots();

            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
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

        private void PokemonSlotElementSubmitted(PokemonSlotElement element)
        {
            switch (_state)
            {
                case ActiveState:
                    {
                        if (element.PokemonSlot != null && element.PokemonSlot.Pokemon != null && element.PokemonSlot.Pokemon.PokemonData != null)
                        {
                            SetState(new PopupState(this, element));
                        }
                        break;
                    }
                case MovePokemonState _movepokemonstate:
                    {
                        _movepokemonstate.PokemonSlotElementSubmitted(element);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Start()
        {
            SetState(new ActiveState(this, _closebutton));
        }

        

        

        private class PopupState : State
        {
            private readonly ManagePokemonWindow _owner;
            private readonly PokemonSlotElement _pokemonslotelement;
            private PokemonSlotPopup _pokemonslotpopup;

            public PopupState(ManagePokemonWindow owner, PokemonSlotElement pokemonslotelement)
            {
                _owner = owner;
                _pokemonslotelement = pokemonslotelement;
            }

            public override void Start()
            {
                _pokemonslotpopup = Instantiate(_owner._popupprefab);
                _pokemonslotpopup.SummaryButtonClicked += () => _owner.SetState(new PokemonSummaryState(_owner, _pokemonslotelement));
                _pokemonslotpopup.MoveButtonClicked += () => _owner.SetState(new MovePokemonState(_owner, _pokemonslotelement));
                _pokemonslotpopup.CloseButtonClicked += () => _owner.SetState(new ActiveState(_owner, _pokemonslotelement.Button));

                _owner.DisableButtons();
            }

            public override void End()
            {
                Destroy(_pokemonslotpopup.gameObject);
                
                //_owner.EnableButtons(_pokemonslotelement);
            }
        }
        private class PokemonSummaryState : State
        {
            private readonly ManagePokemonWindow _owner;
            private readonly PokemonSlotElement _pokemonslotelement;
            private PokemonSummaryWindow _pokemonsummarywindow;

            public PokemonSummaryState(ManagePokemonWindow owner, PokemonSlotElement pokemonslotelement)
            {
                _owner = owner;
                _pokemonslotelement = pokemonslotelement;
            }

            public override void Start()
            {
                _pokemonsummarywindow = Instantiate(_owner._summarywindowprefab);
                _pokemonsummarywindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _pokemonslotelement.Button));
                _pokemonsummarywindow.SetPokemon(_pokemonslotelement.PokemonSlot.Pokemon);
                
                _owner.DisableButtons();
                
            }

            public override void End()
            {
                Destroy(_pokemonsummarywindow.gameObject);
                
                //_owner.EnableButtons(_pokemonslotelement);
            }
        }


        private class MovePokemonState : State
        {
            private readonly ManagePokemonWindow _owner;
            private readonly PokemonSlotElement _pokemonslotelement;

            public MovePokemonState(ManagePokemonWindow owner, PokemonSlotElement pokemonslotelement)
            {
                _owner = owner;
                _pokemonslotelement = pokemonslotelement;
            }

            public override void Start()
            {
                _owner.EnableButtons(_pokemonslotelement.Button);
                //_pokemonslotelement.AddToClassList("selected");
            }

            public override void End()
            {
                _owner.DisableButtons();
                //_pokemonslotelement.RemoveFromClassList("selected");
            }

            public void PokemonSlotElementSubmitted(PokemonSlotElement element)
            {
                (_pokemonslotelement.PokemonSlot.Pokemon, element.PokemonSlot.Pokemon) = (element.PokemonSlot.Pokemon, _pokemonslotelement.PokemonSlot.Pokemon);

                _owner.SetState(new ActiveState(_owner, element.Button));
            }
        }




    }
}
