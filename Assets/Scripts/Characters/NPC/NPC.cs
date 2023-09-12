using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class NPC : Character, IInteractable
    {
        public List<string> mylines = new();
        public GameObject _healeffectprefab;

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

            StartCoroutine(HealRoutine());
        }

        private IEnumerator HealRoutine()
        {
            Time.timeScale = 0;
            IsBusy = true;
            

            DialogWindow window = Instantiate(Prefabs.Instance._dialogwindowprefab, WindowManager.Instance.transform);
            yield return window.ShowLineRoutine("Let me heal your Pokemon!");
            //yield return DialogManager.Instance.DisplayLineRoutine("Let me heal your Pokemon!");
            window.CloseWindow();

            yield return LoadEffect.Instance.CircleRoutine(PartyManager.Instance.HealEveryone);
            //Party.Instance.HealEveryone();
            //Instantiate(_healeffectprefab, transform);

            //yield return new WaitForSeconds(1);

            IsBusy = false;
            Time.timeScale = 1;
        }
    }
}
