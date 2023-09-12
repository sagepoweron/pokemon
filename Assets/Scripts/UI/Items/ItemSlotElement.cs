using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame
{
    public class ItemSlotElement : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _sprite;
        [SerializeField] private Text _name;
        [SerializeField] private Text _amount;
        private ItemAmountSlot _itemamountslot;

        public Button Button => _button;
        public ItemAmountSlot Slot => _itemamountslot;

        public event Action<ItemSlotElement> Selected;
        public event Action<ItemSlotElement> Deselected;
        public event Action<ItemSlotElement> Submitted;

        private void OnDestroy()
        {
            if (_itemamountslot != null)
            {
                _itemamountslot.Changed -= OnInventorySlotChanged;
            }
        }

        private void OnInventorySlotChanged(ItemAmountSlot slot)
        {
            UpdateImage();
        }

        public void Initialize(ItemAmountSlot inventoryslot)
        {
			_itemamountslot = inventoryslot;
			_itemamountslot.Changed += OnInventorySlotChanged;
        }

        private void Start()
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (_itemamountslot != null && _itemamountslot.ItemAmount != null && _itemamountslot.ItemAmount.Item != null)
            {
                _sprite.sprite = _itemamountslot.ItemAmount.Item.Sprite;
                _name.text = _itemamountslot.ItemAmount.Item.Name;
                _amount.text = _itemamountslot.ItemAmount.Amount.ToString();

                _sprite.enabled = true;
                _name.enabled = true;
            }
            else
            {
                _sprite.enabled = false;
                _name.enabled = false;
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            Selected?.Invoke(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Deselected?.Invoke(this);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Submitted?.Invoke(this);
        }
    }
}
