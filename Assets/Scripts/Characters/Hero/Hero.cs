using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class Hero : Character
    {
        private void Start()
        {
            EnableInput();
            SetState(new IdleState(this));
        }

        private class IdleState : State
        {
            private readonly Hero _owner;
            public IdleState(Hero owner)
            {
                _owner = owner;
            }

            //private readonly Hero _owner;

            //public IdleState(Hero owner)
            //{
            //    //_owner = owner;
            //}

            public override void InputA()
            {
                _owner.CastInteract();
            }
            public override void InputB()
            {
            }
            public override void InputX()
            {
                //_owner.UseItem(ItemInventory.Instance.SlotX.Item);
            }
            public override void InputY()
            {
                //_owner.UseItem(ItemInventory.Instance.SlotY.Item);
            }
            public override void StartPressed()
            {
                if (!_owner.ismoving)
                {
                    WindowManager.Instance.Pause();
                }
            }
            public override void Update()
            {
                if (!_owner.ismoving)
                {
                    if (_owner.InputDirection != Vector2.zero)
                    {
                        _owner.Move(_owner.InputDirection, _owner.walkspeed);
                    }
                }


                //if (_owner._inputdirection != Vector2.zero)
                //{
                //    _owner._direction = new(_owner._inputdirection.x, 0, _owner._inputdirection.y);
                //    _owner._movespeed = _owner._walkingspeed;
                //}
                //else
                //{
                //    _owner._movespeed = 0;
                //}

                //if (_owner._isonwater)
                //{
                //    _owner.SetState(new SwimmingState(_owner));
                //}
            }
        }
    }
}