using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

namespace Battle
{
    public class BattleWindow : MonoBehaviour
    {
        [SerializeField] private BattlePokemonSlot _playerunit;
        [SerializeField] private BattlePokemonSlot _enemyunit;
        [SerializeField] private ActionSelectWindow _actionselectwindowprefab;
        [SerializeField] private PokemonSelectWindow _pokemonselectwindowprefab;
        [SerializeField] private Transform _pokeball;
        [SerializeField] private Vector3 _pokeballorigin;
        [SerializeField] private float _pokeballspeed = 8;

        [Header("Audio")]
        [SerializeField] AudioClip _wildpokemonbattlemusic;
        [SerializeField] AudioClip _trainerbattlemusic;
        [SerializeField] AudioClip _victorymusic;

        private Pokemon _enemypokemon;
        private int _turncounter;
        public event Action EndOfBattle;

        public BattlePokemonSlot PlayerUnit => _playerunit;
        public BattlePokemonSlot EnemyUnit => _enemyunit;


        private State _state;
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

        


        public void Initialize(Pokemon pokemon)
        {
            _enemypokemon = pokemon;
        }

        private void Start()
        {
            SetState(new StartState(this));
        }
        
        public void EndBattle()
        {
            StopAllCoroutines();
            AudioManager.Instance.StopMusic();
            EndOfBattle?.Invoke();
        }
        
        
        private class StartState : State
        {
            private readonly BattleWindow _owner;
            private Coroutine _coroutine;
            public StartState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _coroutine = _owner.StartCoroutine(Routine());
            }

            public override void End()
            {
                if (_coroutine != null)
                {
                    _owner.StopCoroutine(_coroutine);
                }
            }

            private IEnumerator Routine()
            {
                AudioManager.Instance.PlayMusic(_owner._wildpokemonbattlemusic);
                _owner._enemyunit.SetPokemon(_owner._enemypokemon);
                yield return _owner._enemyunit.EnterRoutine();
                yield return _owner.ShowLineRoutine($"A wild {_owner._enemyunit.Pokemon.PokemonData.Name} appeared!");

                Pokemon pokemon = PartyManager.Instance.GetFirstHealthyPokemon();
                if (Pokemon.CanBattle(pokemon))
                {
                    _owner._playerunit.SetPokemon(pokemon);
                    yield return _owner._playerunit.EnterRoutine();
                    yield return _owner.ShowLineRoutine($"Go {_owner._playerunit.Pokemon.PokemonData.Name}!");

                    _owner.SetState(new StartTurnState(_owner));
                }
                else
                {
                    yield return _owner.ShowLineRoutine($"You have no useable Pokemon!");
                    yield return _owner.ShowLineRoutine($"You ran away.");
                    _owner.EndBattle();
                }
            }
        }
        private class StartTurnState : State
        {
            private readonly BattleWindow _owner;
            public StartTurnState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _owner._turncounter += 1;
                Debug.Log($"turn {_owner._turncounter}");

                _owner._playerunit.SelectedAction = null;
                _owner._enemyunit.SelectedAction = null;
                _owner.SetState(new GetPlayerActionState(_owner));
            }

            public override void End()
            {
            }
        }
        private class GetPlayerActionState : State
        {
            private readonly BattleWindow _owner;
            private ActionSelectWindow _actionselectwindow;
            public GetPlayerActionState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _actionselectwindow = Instantiate(_owner._actionselectwindowprefab);
                void OnBattleActionSubmitted(BattleAction battleaction)
                {
                    _owner._playerunit.SelectedAction = battleaction;
                    _owner.SetState(new GetEnemyActionState(_owner));
                }
                _actionselectwindow.BattleActionSubmitted += OnBattleActionSubmitted;
                _actionselectwindow.Initialize(_owner);
            }

            public override void End()
            {
                _actionselectwindow.CloseWindow();
            }
        }
        private class GetEnemyActionState : State
        {
            private readonly BattleWindow _owner;
            public GetEnemyActionState(BattleWindow owner)
            {
                _owner = owner;
            }
            public override void Start()
            {
                //get random move for enemy
                int i = UnityEngine.Random.Range(0, _owner._enemyunit.Pokemon.Moves.Count);
                _owner._enemyunit.SelectedAction = new MoveBattleAction(_owner._enemyunit, _owner._playerunit, _owner._enemyunit.Pokemon.Moves[i]);
                _owner.SetState(new BattleState(_owner));
            }
            public override void End()
            {
            }
        }
        private class BattleState : State
        {
            private readonly BattleWindow _owner;
            private Coroutine _coroutine;
            public BattleState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _coroutine = _owner.StartCoroutine(Routine());
            }

            public override void End()
            {
                if (_coroutine != null)
                {
                    _owner.StopCoroutine(_coroutine);
                }
            }

            private IEnumerator Routine()
            {
                //Debug.Log($"player priority: {_playeraction.Priority}");
                //Debug.Log($"enemy priority: {_enemyaction.Priority}");

                bool playergoesfirst;
                if (_owner._playerunit.SelectedAction.Priority == _owner._enemyunit.SelectedAction.Priority)
                {
                    playergoesfirst = _owner._playerunit.SelectedAction.Speed >= _owner._enemyunit.SelectedAction.Speed;
                }
                else
                {
                    playergoesfirst = _owner._playerunit.SelectedAction.Priority >= _owner._enemyunit.SelectedAction.Priority;
                }

                if (playergoesfirst)
                {
                    yield return BattleActionRoutine(_owner._playerunit.SelectedAction);
                    yield return BattleActionRoutine(_owner._enemyunit.SelectedAction);
                }
                else
                {
                    yield return BattleActionRoutine(_owner._enemyunit.SelectedAction);
                    yield return BattleActionRoutine(_owner._playerunit.SelectedAction);
                }

                if (!Pokemon.CanBattle(_owner._playerunit.Pokemon) && PartyManager.Instance.HasHealthyPokemon())
                {
                    _owner.SetState(new ReplacePokemonState(_owner));
                }
                else
                {
                    _owner.SetState(new EndTurnState(_owner));
                }
            }

            private IEnumerator BattleActionRoutine(BattleAction battleaction)
            {
                switch (battleaction)
                {
                    case CatchBattleAction catchaction:
                        {
                            yield return CatchRoutine(catchaction.Target, catchaction.Item);
                            break;
                        }
                    case FleeBattleAction fleeaction:
                        {
                            yield return FleeRoutine(fleeaction.User);
                            break;
                        }
                    case ItemBattleAction itemaction:
                        {
                            yield return ItemRoutine(itemaction.User, itemaction.Item);
                            break;
                        }
                    case MoveBattleAction moveaction:
                        {
                            yield return MoveRoutine(moveaction.User, moveaction.Target, moveaction.Move);
                            break;
                        }
                    case SwitchBattleAction switchaction:
                        {
                            yield return SwitchRoutine(switchaction.User, switchaction.Pokemon);
                            break;
                        }
                }
            }
            private IEnumerator CatchRoutine(BattlePokemonSlot target, ItemAmount item)
            {
                if (item.Item is Pokeball pokeball)
                {
                    yield return _owner.ShowLineRoutine($"You threw a {item.Item.Name}.");
                    item.Amount -= 1;

                    float capturerate = (float)(3 * target.Pokemon.MaxHP - 2 * target.Pokemon.HP) / (3 * target.Pokemon.MaxHP) * 4096 * target.Pokemon.PokemonData.Capturerate * pokeball.CatchRate; //* pokemon.status
                    //float probability_gen4 = 1048560 / MathF.Sqrt(Mathf.Sqrt(16711680 / capturerate));
                    float probability_gen5 = 65536 * (float)Math.Pow(capturerate / 1044480, 0.1875f);
                    //Debug.Log("Capture Rate: " + capturerate);
                    //Debug.Log("Probability: " + probability_gen4);
                    //Debug.Log("Probability2: " + probability_gen5);

                    bool iscaught = true;

                    yield return ThrowPokeballRoutine();
                    for (int i = 0; i < 4; i++)
                    {
                        //yield return _owner.ShowLineRoutine($"Shake: {i + 1}");
                        yield return new WaitForSecondsRealtime(1);
                        yield return ShakePokeballRoutine();

                        int random = UnityEngine.Random.Range(0, 65535);
                        if (random >= probability_gen5)
                        {
                            iscaught = false;
                            _owner._pokeball.position = _owner._pokeballorigin;
                            _owner._enemyunit.SR.enabled = true;
                            yield return _owner.ShowLineRoutine($"The Pokemon broke out!");
                            break;
                        }
                    }

                    if (iscaught)
                    {
                        if (PartyManager.Instance.Add(target.Pokemon))
                        {
                            yield return _owner.ShowLineRoutine($"You caught {target.Pokemon.PokemonData.Name}.");
                        }
                        else
                        {
                            yield return _owner.ShowLineRoutine($"You caught {target.Pokemon.PokemonData.Name} but your party is full.");
                        }

                        _owner.EndBattle();
                    }
                    else
                    {
                        yield return _owner.ShowLineRoutine($"You were unable to catch {target.Pokemon.PokemonData.Name}.");
                    }

                    //float chance = 45 * pokeball.CatchRate / 255;
                    //float success = UnityEngine.Random.value;
                    ////Debug.Log($"chance: {chance}, success: {success}");

                    //if (success < chance)
                    //{
                    //    if (PartyManager.Instance.Add(target.Pokemon))
                    //    {
                    //        yield return _owner.ShowLineRoutine($"You caught {target.Pokemon.PokemonData.Name}.");
                    //    }
                    //    else
                    //    {
                    //        yield return _owner.ShowLineRoutine($"You caught {target.Pokemon.PokemonData.Name} but your party is full.");
                    //    }

                    //    _owner.EndBattle();
                    //}
                    //else
                    //{
                    //    yield return _owner.ShowLineRoutine($"You were unable to catch {target.Pokemon.PokemonData.Name}.");
                    //}
                }
            }
            private IEnumerator ThrowPokeballRoutine()
            {
                _owner._pokeball.position = _owner._pokeballorigin;

                Vector3 target = _owner._enemyunit.transform.position;
                target.y -= 0.5f;

                while (_owner._pokeball.position != target)
                {
                    _owner._pokeball.position = Vector3.MoveTowards(_owner._pokeball.position, target, _owner._pokeballspeed * Time.unscaledDeltaTime);
                    yield return null;
                }

                _owner._enemyunit.SR.enabled = false;
            }
            private IEnumerator ShakePokeballRoutine()
            {
                int angle = 0;

                while (angle > -30)
                {
                    angle--;
                    _owner._pokeball.Rotate(0, 0, -1);
                    yield return null;
                }
                while (angle < 30)
                {
                    angle++;
                    _owner._pokeball.Rotate(0, 0, 1);
                    yield return null;
                }
                while (angle > 0)
                {
                    angle--;
                    _owner._pokeball.Rotate(0, 0, -1);
                    yield return null;
                }
            }





            private IEnumerator FleeRoutine(BattlePokemonSlot user)
            {
                yield return _owner.ShowLineRoutine($"{user.Pokemon.PokemonData.Name} ran away.");

                _owner.EndBattle();
            }
            private IEnumerator ItemRoutine(Pokemon user, ItemAmount item)
            {
                if (item.Item is Medicine medicine)
                {
                    yield return _owner.ShowLineRoutine($"You used {item.Item.Name} on {user.PokemonData.Name}.");
                    user.HP += medicine.RecoverAmount;
                    item.Amount -= 1;

                    if (_owner._playerunit.Pokemon == user)
                    {
                        yield return _owner._playerunit.HealRoutine();
                    }
                }
            }
            private IEnumerator MoveRoutine(BattlePokemonSlot user, BattlePokemonSlot target, Move move)
            {
                if (!Pokemon.CanBattle(user.Pokemon) || !Pokemon.CanBattle(target.Pokemon))
                {
                    yield break;
                }
                yield return _owner.ShowLineRoutine($"{user.Pokemon.PokemonData.Name} used {move.MoveData.Name}.");

                yield return user.AttackRoutine(move);

                (float effectiveness, bool criticalhit) = DamageCalculation.ApplyDamage(user, target, move);

                yield return target.HitRoutine();

                if (effectiveness > 1)
                {
                    yield return _owner.ShowLineRoutine("It's super effective!");
                }
                else if (effectiveness < 1)
                {
                    yield return _owner.ShowLineRoutine("It's not very effective!");
                }
                if (criticalhit)
                {
                    yield return _owner.ShowLineRoutine("Critical hit!");
                }

                for (int i = 0; i < move.MoveData.UserEffects.Count; i++)
                {
                    yield return _owner.ShowLineRoutine(move.MoveData.UserEffects[i].ApplyEffect(user));
                }
                for (int i = 0; i < move.MoveData.TargetEffects.Count; i++)
                {
                    yield return _owner.ShowLineRoutine(move.MoveData.TargetEffects[i].ApplyEffect(target));
                }

                user.UpdateUI();
                target.UpdateUI();

                if (target.Pokemon.HP <= 0)
                {
                    yield return target.FaintRoutine();
                    yield return _owner.ShowLineRoutine($"{target.Pokemon.PokemonData.Name} fainted!");
                }
            }
            private IEnumerator SwitchRoutine(BattlePokemonSlot user, Pokemon pokemon)
            {
                if (Pokemon.CanBattle(user.Pokemon))
                {
                    yield return user.FaintRoutine();
                }

                user.SetPokemon(pokemon);

                yield return user.EnterRoutine();
            }
        }
        private class ReplacePokemonState : State
        {
            private readonly BattleWindow _owner;
            private PokemonSelectWindow _pokemonselectwindow;
            private Coroutine _routine;
            public ReplacePokemonState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                _pokemonselectwindow = Instantiate(_owner._pokemonselectwindowprefab);
                void OnPokemonSubmitted(PokemonSlot slot)
                {
                    if (Pokemon.CanBattle(slot.Pokemon))
                    {
                        _pokemonselectwindow.CloseWindow();
                        _routine = _owner.StartCoroutine(Routine(slot.Pokemon));
                    }
                }
                _pokemonselectwindow.PokemonSlotSelected += OnPokemonSubmitted;
            }

            public override void End()
            {
                if (_routine != null)
                {
                    _owner.StopCoroutine(_routine);
                }
            }

            private IEnumerator Routine(Pokemon pokemon)
            {
                _owner._playerunit.SetPokemon(pokemon);
                yield return _owner._playerunit.EnterRoutine();
                yield return _owner.ShowLineRoutine($"Go {_owner._playerunit.Pokemon.PokemonData.Name}!");
                _owner.SetState(new EndTurnState(_owner));
            }
        }
        private class EndTurnState : State
        {
            private readonly BattleWindow _owner;
            private Coroutine _routine;
            public EndTurnState(BattleWindow owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                if (Pokemon.CanBattle(_owner._playerunit.Pokemon))
                {
                    if (Pokemon.CanBattle(_owner._enemyunit.Pokemon))
                    {
                        _owner.SetState(new StartTurnState(_owner));
                    }
                    else
                    {
                        _routine = _owner.StartCoroutine(WinRoutine());
                    }
                }
                else
                {
                    _routine = _owner.StartCoroutine(LoseRoutine());
                }
            }

            public override void End()
            {
                if (_routine != null)
                {
                    _owner.StopCoroutine(_routine);
                }
            }

            private IEnumerator LoseRoutine()
            {
                yield return _owner.ShowLineRoutine($"You have no Pokemon to use!");
                _owner.EndBattle();
            }
            private IEnumerator WinRoutine()
            {
                yield return _owner.ShowLineRoutine($"You won!");
                _owner.EndBattle();
            }
        }



        private IEnumerator ShowLineRoutine(string line)
        {
            DialogWindow dialogwindow = Instantiate(Prefabs.Instance._dialogwindowprefab);
            yield return dialogwindow.ShowLineRoutine(line);
            Destroy(dialogwindow.gameObject);
        }





    }
}
