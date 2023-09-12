using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class ItemAmount
    {
        [SerializeField] private ItemData _item;
        [SerializeField] private int _amount;
        public ItemData Item => _item;
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = Mathf.Clamp(value, 0, _item.MaxAmount);
                Changed?.Invoke();
            }
        }

        public event Action Changed;

        public ItemAmount(ItemData item, int amount)
        {
            _item = item;
            _amount = amount;
        }
        public ItemAmount(ItemAmount itemamount)
        {
            _item = itemamount._item;
            _amount = itemamount._amount;
        }

        public bool IsFull => _amount >= _item.MaxAmount;

        public static bool HasItem(ItemAmount itemamount)
        {
            return itemamount != null && itemamount.Item != null && itemamount.Amount > 0;
        }
    }
}
