using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ManageItemsWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Text _itemname;
        [SerializeField] private Text _itemdescription;
        [SerializeField] private Button _healtab;
        [SerializeField] private Button _pokeballstab;
        [SerializeField] private CanvasGroup _healpage;
        [SerializeField] private CanvasGroup _pokeballspage;
        [SerializeField] private ItemSlotElement _itemslottemplate;
        [SerializeField] private PokemonSelectWindow _pokemonselectwindowprefab;

        public event Action CloseClicked;

        private void OnEnable()
        {
            LoadHealPage();
            LoadPokeballsPage();

            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        private void Start()
        {
            SetState(new ActiveState(this, _closebutton));
        }

        private void LoadHealPage()
        {
            for (int i = 0; i < InventoryManager.Instance.MedicineInventory.Slots.Count; i++)
            {
                ItemSlotElement itemslotelement = Instantiate(_itemslottemplate, _healpage.transform);
                itemslotelement.Initialize(InventoryManager.Instance.MedicineInventory.Slots[i]);
                itemslotelement.Submitted += OnItemSlotSubmitted;
                itemslotelement.Selected += OnItemSlotFocused;
                itemslotelement.Deselected += OnItemSlotUnfocused;
            }

            void OnClick()
            {
                _healpage.alpha = 1;
                _healpage.interactable = true;

                _pokeballspage.alpha = 0;
                _pokeballspage.interactable = false;
            }

            _healtab.onClick.AddListener(OnClick);
        }

        private void LoadPokeballsPage()
        {
            for (int i = 0; i < InventoryManager.Instance.PokeballInventory.Slots.Count; i++)
            {
                ItemSlotElement itemslotelement = Instantiate(_itemslottemplate, _pokeballspage.transform);
                itemslotelement.Initialize(InventoryManager.Instance.PokeballInventory.Slots[i]);
                itemslotelement.Submitted += OnItemSlotSubmitted;
                itemslotelement.Selected += OnItemSlotFocused;
                itemslotelement.Deselected += OnItemSlotUnfocused;
            }

            void PokeballsTabClicked()
            {
                _healpage.alpha = 0;
                _healpage.interactable = false;

                _pokeballspage.alpha = 1;
                _pokeballspage.interactable = true;
            }
            _pokeballstab.onClick.AddListener(PokeballsTabClicked);
        }

        private void OnItemSlotFocused(ItemSlotElement slot)
        {
            UpdateDescription(slot);
        }
        private void OnItemSlotUnfocused(ItemSlotElement slot)
        {
            UpdateDescription(null);
        }

        private void OnItemSlotSubmitted(ItemSlotElement slot)
        {
            if (slot.Slot == null || slot.Slot.ItemAmount == null || slot.Slot.ItemAmount.Item == null)
            {
                return;
            }

            switch (slot.Slot.ItemAmount.Item)
            {
                case Medicine:
                    {
                        SetState(new UseItemState(this, slot));
                        break;
                    }
                case Pokeball:
                    {
                        break;
                    }
            }
        }

        private void UpdateDescription(ItemSlotElement slot)
        {
            if (slot != null && slot.Slot != null && slot.Slot.ItemAmount != null && slot.Slot.ItemAmount.Item != null)
            {
                _itemname.text = slot.Slot.ItemAmount.Item.Name;
                _itemdescription.text = slot.Slot.ItemAmount.Item.Description;
            }
            else
            {
                _itemname.text = "";
                _itemdescription.text = "";
            }
        }

        private class UseItemState : State
        {
            private readonly ManageItemsWindow _owner;
            private readonly ItemSlotElement _itemslot;
            private PokemonSelectWindow _pokemonselectwindow;
            
            public UseItemState(ManageItemsWindow owner, ItemSlotElement itemslot)
            {
                _owner = owner;
                _itemslot = itemslot;
            }

            public override void Start()
            {
                _pokemonselectwindow = Instantiate(_owner._pokemonselectwindowprefab);
                _pokemonselectwindow.CloseClicked += () => _owner.SetState(new ActiveState(_owner, _itemslot.Button));

                void OnPokemonSlotSelected(PokemonSlot pokemonslot)
                {
                    if (_itemslot.Slot.ItemAmount.Item is Medicine medicine && _itemslot.Slot.ItemAmount.Amount > 0 && pokemonslot.Pokemon != null && pokemonslot.Pokemon.PokemonData != null && pokemonslot.Pokemon.HP < pokemonslot.Pokemon.MaxHP)
                    {
                        pokemonslot.Pokemon.HP += medicine.RecoverAmount;
                        _itemslot.Slot.ItemAmount.Amount -= 1;
                        _owner.SetState(new ActiveState(_owner, _itemslot.Button));
                    }
                }
                _pokemonselectwindow.PokemonSlotSelected += OnPokemonSlotSelected;
            }

            public override void End()
            {
                _pokemonselectwindow.CloseWindow();
            }
        }
    }
}
