using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BattleAction
    {
        public int Priority = 5;
        public int Speed;
    }

    public class ItemBattleAction : BattleAction
    {
        private readonly Pokemon _user;
        private readonly ItemAmount _item;

        public Pokemon User => _user;
        public ItemAmount Item => _item;

        public ItemBattleAction(Pokemon user, ItemAmount item)
        {
            _user = user;
            _item = item;
        }
    }

    public class FleeBattleAction: BattleAction
    {
        private readonly BattlePokemonSlot _user;
        public BattlePokemonSlot User => _user;

        public FleeBattleAction(BattlePokemonSlot user)
        {
            _user = user;
        }
    }

    public class CatchBattleAction: BattleAction
    {
        private readonly BattlePokemonSlot _target;
        private readonly ItemAmount _item;

        public BattlePokemonSlot Target => _target;
        public ItemAmount Item => _item;

        public CatchBattleAction(BattlePokemonSlot target, ItemAmount item)
        {
            _target = target;
            _item = item;
        }
    }

    public class SwitchBattleAction: BattleAction
    {
        private readonly BattlePokemonSlot _user;
        private readonly Pokemon _pokemon;
        public BattlePokemonSlot User => _user;
        public Pokemon Pokemon => _pokemon;

        public SwitchBattleAction(BattlePokemonSlot user, Pokemon pokemon)
        {
            _user = user;
            _pokemon = pokemon;
        }
    }

    public class MoveBattleAction : BattleAction
    {
        private readonly BattlePokemonSlot _user;
        private readonly BattlePokemonSlot _target;
        private readonly Move _move;

        public BattlePokemonSlot User => _user;
        public BattlePokemonSlot Target => _target;
        public Move Move => _move;

        public MoveBattleAction(BattlePokemonSlot user, BattlePokemonSlot target, Move move)
        {
            Priority = move.MoveData.Priority;
            Speed = user.Speed.GetValue();
            _user = user;
            _target = target;
            _move = move;
        }
    }
}
