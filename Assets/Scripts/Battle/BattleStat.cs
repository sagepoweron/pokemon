using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BattleStat
    {
        private readonly int _value;
        private int _stage;

        public int Stage => _stage;

        public BattleStat(int value)
        {
            _value = value;
            _stage = 0;
        }

        public bool Raise()
        {
            if (_stage < 6)
            {
                _stage += 1;
                return true;
            }
            return false;
        }
        public bool Lower()
        {
            if (_stage > -6)
            {
                _stage -= 1;
                return true;
            }
            return false;
        }

        public int GetValue()
        {
            float multiplier = 1 + Mathf.Abs(_stage) * 0.5f;

            switch (_stage)
            {
                case > 0:
                    {
                        return Mathf.FloorToInt(_value * multiplier);
                    }
                case < 0:
                    {
                        return Mathf.FloorToInt(_value / multiplier);
                    }
                default:
                    {
                        return _value;
                    }
            }
        }
    }
}
