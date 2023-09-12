using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class Move
    {
        private readonly MoveData _movedata;
        private int _pp;

        public MoveData MoveData => _movedata;
        public int PP
        {
            get => _pp;
            set => _pp = value;
        }

        public Move(MoveData movedata)
        {
            _movedata = movedata;
            _pp = movedata.MaxPP;
        }

        public void Restore()
        {
            _pp = _movedata.MaxPP;
        }

        public static bool CanUse(Move move)
        {
            return move != null && move.MoveData != null && move.PP > 0;
        }
    }
}
