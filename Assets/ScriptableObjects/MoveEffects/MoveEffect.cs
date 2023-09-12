using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    //[CreateAssetMenu(menuName = "ScriptableObject/Battle/MoveEffect")]
    public abstract class MoveEffect : ScriptableObject
    {
        public abstract string ApplyEffect(BattlePokemonSlot target);
    }
}
