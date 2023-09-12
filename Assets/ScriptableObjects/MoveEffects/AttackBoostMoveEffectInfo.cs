using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/MoveEffect/AttackBoost")]
    public class AttackBoostMoveEffectInfo : MoveEffect
    {
        [SerializeField] private int _boost;

        public override string ApplyEffect(BattlePokemonSlot target)
        {
            if (_boost > 0)
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
            else if (_boost < 0)
            {
                if (target.Attack.Lower())
                {
                    return $"{target.Pokemon.PokemonData.Name}'s attack was lowered!";
                }
                else
                {
                    return $"{target.Pokemon.PokemonData.Name}'s attack can't go any lower.";
                }
            }
            return "There was no effect.";
        }
    }
}
