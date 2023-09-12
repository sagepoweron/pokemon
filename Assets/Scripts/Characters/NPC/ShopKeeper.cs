using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopKeeper : Character, IInteractable
    {
        [SerializeField] private List<string> _lines = new();
        [SerializeField] private DialogWindow _dialogwindowprefab;
        [SerializeField] private List<ItemAmount> _itemamounts = new();

        private void Update()
        {
            UpdateAnimation();
        }

        public void Interact(Vector2 direction)
        {
            if (IsBusy)
            {
                return;
            }

            Direction = -direction;

            StartCoroutine(DialogRoutine());
        }

        private IEnumerator DialogRoutine()
        {
            Time.timeScale = 0;
            IsBusy = true;


            DialogWindow window = Instantiate(_dialogwindowprefab);
            yield return window.ShowDialog(_lines);
            window.CloseWindow();

            foreach (var itemamount in _itemamounts)
            {
                InventoryManager.Instance.Add(itemamount);
            }
            
            IsBusy = false;
            Time.timeScale = 1;
        }
    }
}
