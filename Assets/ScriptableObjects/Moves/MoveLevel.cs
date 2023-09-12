using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class MoveDataLevel
    {
        [SerializeField] private MoveData _movedata;
        [SerializeField] private int _level;

        public MoveData MoveData => _movedata;
        public int Level => _level;

        public MoveDataLevel(MoveData movedata, int level)
        {
            _movedata = movedata;
            _level = level;
        }
    }
}
