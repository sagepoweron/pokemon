using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace MyGame
{
    [Serializable]
    public class ItemAmountSlot
    {
        [SerializeField] private ItemAmount _itemamount;
        public event Action<ItemAmountSlot> Changed;

        public ItemAmount ItemAmount
        {
            get => _itemamount;
            set
            {
                if (_itemamount != null)
                {
                    _itemamount.Changed -= OnItemAmountChanged;
                }
                _itemamount = value;
                if (_itemamount != null)
                {
                    _itemamount.Changed += OnItemAmountChanged;
                }
            }
        }

        private void OnItemAmountChanged()
        {
            Changed?.Invoke(this);
        }

        public void Debug2()
        {
            Debug.Log(Changed.GetInvocationList().Length);
        }
    }
}
