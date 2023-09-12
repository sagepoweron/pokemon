using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DamageCalculation
    {
        private static float GetSameTypeAttackBonus(Pokemon user, Move move)
        {
            return move.MoveData.ElementType == user.PokemonData.Type1 || move.MoveData.ElementType == user.PokemonData.Type2
                ? 1.5f : 1;
        }
        private static float GetTypeEffectivenessMultiplier(Pokemon target, Move move)
        {
            float typemultiplier1 = 1;
            float typemultiplier2 = 1;

            TypeData movetype = move.MoveData.ElementType;
            TypeData targettype1 = target.PokemonData.Type1;
            TypeData targettype2 = target.PokemonData.Type2;

            for (int i = 0; i < movetype.TypeDataEffectivenessList.Count; i++)
            {
                if (movetype.TypeDataEffectivenessList[i].TypeData == targettype1)
                {
                    typemultiplier1 = movetype.TypeDataEffectivenessList[i].Effectiveness;
                    break;
                }
            }
            
            for (int i = 0; i < movetype.TypeDataEffectivenessList.Count; i++)
            {
                if (movetype.TypeDataEffectivenessList[i].TypeData == targettype2)
                {
                    typemultiplier2 = movetype.TypeDataEffectivenessList[i].Effectiveness;
                    break;
                }
            }

            //Debug.Log($"multiplier 1 = {typemultiplier1}, multiplier2 = {typemultiplier2}");

            return typemultiplier1 * typemultiplier2;
        }


        public static (float effectiveness, bool criticalhit) ApplyDamage(BattlePokemonSlot user, BattlePokemonSlot target, Move move)
        {
            if (user == null || user == null || user.Pokemon == null || target == null || target == null || target.Pokemon == null || move == null)
            {
                return (0, false);
            }


            move.PP -= 1;

            //attacker and defender
            float a = 2 * user.Pokemon.Level / 5f + 2;
            float b = 0;
            if (move.MoveData.Category == MoveCategory.physical)
            {
                b = (float)user.Attack.GetValue() / target.Defense.GetValue();
            }
            else if (move.MoveData.Category == MoveCategory.special)
            {
                b = (float)user.SpecialAttack.GetValue() / target.SpecialDefense.GetValue();
            }
            else if (move.MoveData.Category == MoveCategory.other)
            {
                return (1, false);
            }

            //move power
            float movebase = move.MoveData.Power;

            //stab
            float sametypeattack = GetSameTypeAttackBonus(user.Pokemon, move);

            //type
            float effectiveness = GetTypeEffectivenessMultiplier(target.Pokemon, move);

            //critical
            float critical = 1;
            if (Random.value * 100 <= 6.5f)
            {
                critical = 1.5f;
            }

            //random
            float random = Random.Range(85, 101) / 100f;

            int damage = Mathf.FloorToInt((a * b * movebase / 50 + 2) * sametypeattack * effectiveness * critical * random);
            target.Pokemon.HP -= damage;
            if (target.Pokemon.HP <= 0)
            {
                target.Pokemon.HP = 0;
            }

            return (effectiveness, critical > 1);
        }



    }
}
