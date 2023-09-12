using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Window : MonoBehaviour
    {
        protected CanvasGroup _canvasgroup;
        protected State _state;

        private void Awake()
        {
            _canvasgroup = GetComponent<CanvasGroup>();
        }

        public class State
        {
            public virtual void Start() { }
            public virtual void End() { }
        }
        public void SetState(State state)
        {
            _state?.End();
            _state = state;
            _state?.Start();
        }

        public void CloseWindow()
        {
            _state?.End();
            Destroy(gameObject);
        }

        public void EnableButtons(Selectable focus)
        {
            _canvasgroup.interactable = true;
            if (focus != null)
            {
                focus.Select();
            }
        }
        public void DisableButtons()
        {
            _canvasgroup.interactable = false;
        }

        public class ActiveState : State
        {
            private readonly Window _owner;
            private readonly Selectable _focus;
            public ActiveState(Window owner, Selectable focus)
            {
                _owner = owner;
                _focus = focus;
            }

            public override void Start()
            {
                _owner.EnableButtons(_focus);
            }

            public override void End()
            {
                _owner.DisableButtons();
            }
        }

        /*private MyInputActions _inputactions;
        public void DisableInput()
        {
            _inputactions.Disable();
        }
        public void EnableInput()
        {
            _inputactions.Enable();
        }

        protected virtual void InputStart(InputAction.CallbackContext value)
        {
        }
        protected virtual void InputA(InputAction.CallbackContext value)
        {
        }
        protected virtual void InputB(InputAction.CallbackContext value)
        {
        }
        protected virtual void InputX(InputAction.CallbackContext value)
        {
        }
        protected virtual void InputY(InputAction.CallbackContext value)
        {
        }*/
    }
}
