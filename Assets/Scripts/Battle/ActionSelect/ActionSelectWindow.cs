using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ActionSelectWindow : Window
    {
        [SerializeField] private Button _fightbutton;
        [SerializeField] private Button _itemsbutton;
        [SerializeField] private Button _switchbutton;
        [SerializeField] private Button _runbutton;
        [SerializeField] private ItemsWindow _itemswindowprefab;
        [SerializeField] private MoveSelectWindow _moveselectwindowprefab;
        [SerializeField] private PokemonSelectWindow _pokemonselectwindowprefab;

        private BattleWindow _owner;
        public event Action<BattleAction> BattleActionSubmitted;

        public void Initialize(BattleWindow owner)
        {
            _owner = owner;
        }

        private void Start()
        {
            SetState(new ActiveState(this, _fightbutton));
        }

        private void OnEnable()
        {
            _fightbutton.onClick.AddListener(() => SetState(new FightState(this)));
            _itemsbutton.onClick.AddListener(() => SetState(new ItemSelectState(this)));
            _switchbutton.onClick.AddListener(() => SetState(new SwitchState(this)));
            _runbutton.onClick.AddListener(Run);
        }

        public void Run()
        {
			BattleActionSubmitted?.Invoke(new FleeBattleAction(_owner.PlayerUnit));
		}

        private class FightState : State
        {
            private readonly ActionSelectWindow _owner;
            private MoveSelectWindow _moveselectwindow;
            public FightState(ActionSelectWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _moveselectwindow = Instantiate(_owner._moveselectwindowprefab);
                _moveselectwindow.SetPokemon(_owner._owner.PlayerUnit.Pokemon);

                void OnMoveSubmitted(Move movepp)
                {
                    if (movepp != null && movepp.PP > 0)
                    {
                        _owner.BattleActionSubmitted?.Invoke(new MoveBattleAction(_owner._owner.PlayerUnit, _owner._owner.EnemyUnit, movepp));
                    }
                }

                _moveselectwindow.MoveSubmitted += OnMoveSubmitted;
                _moveselectwindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _owner._fightbutton));
                
            }

            public override void End()
            {
                _moveselectwindow.CloseWindow();
            }
        }

        private class ItemSelectState : State
        {
            private readonly ActionSelectWindow _owner;
            private ItemsWindow _itemswindow;
            public ItemSelectState(ActionSelectWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _itemswindow = Instantiate(_owner._itemswindowprefab);
                _itemswindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _owner._itemsbutton));

                void OnHealItemSelected(ItemAmount item, Pokemon pokemon)
                {
                    _owner.BattleActionSubmitted?.Invoke(new ItemBattleAction(pokemon, item));
                }

                void OnPokeballItemSelected(ItemAmount item)
                {
                    _owner.BattleActionSubmitted?.Invoke(new CatchBattleAction(_owner._owner.EnemyUnit, item));
                }

                _itemswindow.HealItemSelected += OnHealItemSelected;
                _itemswindow.PokeballItemSelected += OnPokeballItemSelected;
            }

            public override void End()
            {
                _itemswindow.CloseWindow();
            }
        }

        private class SwitchState : State
        {
            private readonly ActionSelectWindow _owner;
            private PokemonSelectWindow _pokemonselectwindow;
            public SwitchState(ActionSelectWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _pokemonselectwindow = Instantiate(_owner._pokemonselectwindowprefab);
                _pokemonselectwindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _owner._switchbutton));

                void OnPokemonSelected(PokemonSlot slot)
                {
                    if (_owner._owner.PlayerUnit.Pokemon != slot.Pokemon && Pokemon.CanBattle(slot.Pokemon))
                    {
                        _owner.BattleActionSubmitted?.Invoke(new SwitchBattleAction(_owner._owner.PlayerUnit, slot.Pokemon));
                    }
                }
                _pokemonselectwindow.PokemonSlotSelected += OnPokemonSelected;
            }

            public override void End()
            {
                _pokemonselectwindow.CloseWindow();
            }
        }

    }
}
