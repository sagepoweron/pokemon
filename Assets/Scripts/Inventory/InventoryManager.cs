using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            LoadInventory(_inventoryinfo);
        }

        public int Gold;
        [SerializeField] private InventoryInfo _inventoryinfo;

        public Inventory MedicineInventory = new();
        public Inventory PokeballInventory = new();

        private void LoadInventory(InventoryInfo inventoryinfo)
        {
            Gold = inventoryinfo.Gold;

            for (int i = 0; i < inventoryinfo.Medicines.Count; i++)
            {
                MedicineInventory.Add(inventoryinfo.Medicines[i]);
            }

            for (int i = 0; i < inventoryinfo.Pokeballs.Count; i++)
            {
                PokeballInventory.Add(inventoryinfo.Pokeballs[i]);
            }
        }

        public bool Add(ItemAmount itemamount)
        {
            if (itemamount == null || itemamount.Item == null)
            {
                return false;
            }

            switch (itemamount.Item)
            {
                case Medicine:
                    return MedicineInventory.Add(itemamount);
                case Pokeball:
                    return PokeballInventory.Add(itemamount);
                default:
                    return false;
            }
        }
    }
}
