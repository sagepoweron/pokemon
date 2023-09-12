using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame
{
    public class PokemonCursorUI
    {
        private readonly VisualElement _sprite;
        private PokemonSlot _slot;

        public PokemonCursorUI(VisualElement root)
        {
            _sprite = root.Q<VisualElement>("sprite");
        }

        public void SetSlot(PokemonSlot slot)
        {
            _slot = slot;

            UpdateImage();
        }


        private void UpdateImage()
        {
            if (_slot != null && _slot.Pokemon != null && _slot.Pokemon.PokemonData != null)
            {
                _sprite.style.backgroundImage = _slot.Pokemon.PokemonData.Sprite.texture;
                _sprite.style.opacity = 1;
            }
            else
            {
                _sprite.style.backgroundImage = null;
                _sprite.style.opacity = 0;
            }

        }
    }
}
