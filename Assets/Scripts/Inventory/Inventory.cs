using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class Inventory
    {
        public List<ItemAmountSlot> Slots = new();

        public bool Add(ItemData item)
        {
            if (item == null)
            {
                return false;
            }

            //only one stack of item allowed
            //try to add to slot of same item
            foreach (ItemAmountSlot itemamountslot in Slots)
            {
                if (itemamountslot.ItemAmount != null && itemamountslot.ItemAmount.Item == item)
                {
                    if (!itemamountslot.ItemAmount.IsFull)
                    {
                        itemamountslot.ItemAmount.Amount += 1;
                        return true;
                    }
                    return false;
                }
            }

            ItemAmountSlot slot = new()
            {
                ItemAmount = new(item, 1)
            };
            slot.Changed += OnSlotChanged;
            Slots.Add(slot);
            return true;
        }

        public bool Add(ItemAmount itemamount)
        {
            if (itemamount == null || itemamount.Item == null)
            {
                return false;
            }

            //only one stack of item allowed
            //try to add to slot of same item
            foreach (ItemAmountSlot itemamountslot in Slots)
            {
                if (itemamountslot.ItemAmount != null && itemamountslot.ItemAmount.Item == itemamount.Item)
                {
                    if (itemamountslot.ItemAmount.IsFull)
                    {
                        return false;
                    }
                    itemamountslot.ItemAmount.Amount += itemamount.Amount;
                    return true;
                }
            }

            ItemAmountSlot slot = new()
            {
                ItemAmount = new(itemamount)
            };
            slot.Changed += OnSlotChanged;
            Slots.Add(slot);
            return true;
        }


        public bool Remove(ItemData item)
        {
            if (item == null)
            {
                return false;
            }

            foreach (ItemAmountSlot itemamountslot in Slots)
            {
                if (itemamountslot.ItemAmount != null && itemamountslot.ItemAmount.Item == item)
                {
                    itemamountslot.ItemAmount.Amount -= 1;
                    return true;
                }
            }
            return false;
        }


        private void OnSlotChanged(ItemAmountSlot slot)
        {
            if (slot.ItemAmount != null && slot.ItemAmount.Amount <= 0)
            {
                slot.Changed -= OnSlotChanged;
                Slots.Remove(slot);
            }
        }

    }
}
