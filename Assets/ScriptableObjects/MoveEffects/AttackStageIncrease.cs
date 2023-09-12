using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/MoveEffect/AttackStageIncrease")]
    public class AttackStageIncrease : MoveEffect
    {
        public override string ApplyEffect(BattlePokemonSlot target)
        {
            if (target.Attack.Raise())
            {
                return $"{target.Pokemon.PokemonData.Name}'s attack rose!";
            }
            else
            {
                return $"{target.Pokemon.PokemonData.Name}'s attack can't go any higher.";
            }
        }
    }
}
