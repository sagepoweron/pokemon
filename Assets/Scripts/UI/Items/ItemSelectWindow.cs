using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ItemSelectWindow : Window
    {
        [SerializeField] private Button _closebutton;
        [SerializeField] private Text _itemname;
        [SerializeField] private Text _itemdescription;
        [SerializeField] private Button _healtab;
        [SerializeField] private Button _pokeballstab;
        [SerializeField] private CanvasGroup _healpage;
        [SerializeField] private CanvasGroup _pokeballspage;
        [SerializeField] private ItemSlotElement _itemslottemplate;
        public event Action CloseClicked;
        public event Action<ItemAmountSlot> HealSlotSelected;
        public event Action<ItemAmountSlot> PokeballSlotSelected;

        private void OnEnable()
        {
            _closebutton.onClick.AddListener(() => CloseClicked?.Invoke());

            LoadHealPage();
            LoadPokeballsPage();
            
            _closebutton.Select();
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
            }

            _healtab.onClick.AddListener(() =>
            {
                _healpage.alpha = 1;
                _healpage.interactable = true;

                _pokeballspage.alpha = 0;
                _pokeballspage.interactable = false;
            });
        }

        private void LoadPokeballsPage()
        {
            for (int i = 0; i < InventoryManager.Instance.PokeballInventory.Slots.Count; i++)
            {
                ItemSlotElement itemslotelement = Instantiate(_itemslottemplate, _pokeballspage.transform);
                itemslotelement.Initialize(InventoryManager.Instance.PokeballInventory.Slots[i]);
                itemslotelement.Submitted += OnItemSlotSubmitted;
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
                        HealSlotSelected?.Invoke(slot.Slot);
                        break;
                    }
                case Pokeball:
                    {
                        PokeballSlotSelected?.Invoke(slot.Slot);
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
    }
}
