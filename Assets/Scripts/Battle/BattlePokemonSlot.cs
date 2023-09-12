using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class BattlePokemonSlot : MonoBehaviour
    {
        private SpriteRenderer _sr;
        [SerializeField] private AudioClip _attackclip;
        [SerializeField] private AudioClip _damagedclip;
        [SerializeField] private bool _isplayer;
        private Vector3 _origin;
        private Pokemon _pokemon;
        private BattleStat _attack;
        private BattleStat _defense;
        private BattleStat _specialattack;
        private BattleStat _specialdefense;
        private BattleStat _speed;
        [SerializeField] private CanvasGroup _canvasgroup;
        [SerializeField] private Text _namelabel;
        [SerializeField] private Text _levellabel;
        [SerializeField] private Text _attacklabel;
        [SerializeField] private Text _defenselabel;
        [SerializeField] private Text _specialattacklabel;
        [SerializeField] private Text _specialdefenselabel;
        [SerializeField] private Text _speedlabel;
        [SerializeField] private HealthBar _healthbarelement;

        public SpriteRenderer SR => _sr;
        public Pokemon Pokemon => _pokemon;
        public BattleStat Attack => _attack;
        public BattleStat Defense => _defense;
        public BattleStat SpecialAttack => _specialattack;
        public BattleStat SpecialDefense => _specialdefense;
        public BattleStat Speed => _speed;
        public BattleAction SelectedAction;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.enabled = false;
            _origin = transform.localPosition;
        }

        public void SetPokemon(Pokemon pokemon)
        {
            _pokemon = pokemon;
            _attack = new(_pokemon.Attack);
            _defense = new(_pokemon.Defense);
            _specialattack = new(_pokemon.SpecialAttack);
            _specialdefense = new(_pokemon.SpecialDefense);
            _speed = new(_pokemon.Speed);

            UpdateImage();
            UpdateUI();
            
        }
        
        private void UpdateImage()
        {
            if (_pokemon != null && _pokemon.PokemonData != null)
            {
                _namelabel.text = _pokemon.PokemonData.Name;
                _levellabel.text = $"L{_pokemon.Level}";

                _healthbarelement.Initialize(_pokemon.MaxHP);
                _healthbarelement.Set(_pokemon.HP);

                _canvasgroup.alpha = 1;
            }
            else
            {
                _canvasgroup.alpha = 0;
            }
        }

        public void UpdateUI()
        {
            if (_pokemon != null && _pokemon.PokemonData != null)// && _pokemoninbattle.Pokemon.HP > 0)
            {
                transform.localPosition = _origin;
                _sr.color = Color.white;
                _sr.enabled = true;
                if (_isplayer)
                {
                    _sr.sprite = _pokemon.PokemonData.BackSprite;
                }
                else
                {
                    _sr.sprite = _pokemon.PokemonData.Sprite;
                }

                _attacklabel.text = $"A{_attack.Stage}";
                _defenselabel.text = $"D{_defense.Stage}";
                _specialattacklabel.text = $"SA{_specialattack.Stage}";
                _specialdefenselabel.text = $"SD{_specialdefense.Stage}";
                _speedlabel.text = $"S{_speed.Stage}";

                _attacklabel.enabled = _attack.Stage != 0;
                _defenselabel.enabled = _defense.Stage != 0;
                _specialattacklabel.enabled = _specialattack.Stage != 0;
                _specialdefenselabel.enabled = _specialdefense.Stage != 0;
                _speedlabel.enabled = _speed.Stage != 0;
            }
            else
            {
                _sr.enabled = false;

                _attacklabel.enabled = false;
                _defenselabel.enabled = false;
                _specialattacklabel.enabled = false;
                _specialdefenselabel.enabled = false;
                _speedlabel.enabled = false;
            }
        }

        public IEnumerator EnterRoutine()
        {
            if (_pokemon == null || _pokemon.PokemonData == null || _pokemon.HP <= 0)
            {
                yield break;
            }
            
            AudioManager.Instance.PlaySFX(_pokemon.PokemonData.Cry);

            float distance = 10;
            float duration = 0.5f;

            if (_isplayer)
            {
                transform.localPosition = _origin - new Vector3(distance, 0);
            }
            else
            {
                transform.localPosition = _origin + new Vector3(distance, 0);
            }

            while (transform.localPosition != _origin)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _origin, distance * Time.unscaledDeltaTime / duration);
                yield return null;
            }

            //finished?.Invoke();
        }

        public IEnumerator FaintRoutine()
        {
            AudioManager.Instance.PlaySFX(_pokemon.PokemonData.Cry);

            float duration = 0.25f;
            Color startcolor = _sr.color;
            Vector3 target = new(_origin.x, _origin.y - 1);

            float tick = 0;
            while (tick < 1)
            {
                tick += Time.unscaledDeltaTime / duration;
                _sr.color = Color.Lerp(startcolor, Color.clear, tick);
                transform.localPosition = Vector3.Lerp(_origin, target, tick);
                yield return null;
            }
        }

        public IEnumerator AttackRoutine(Move move)
        {
            AudioManager.Instance.PlaySFX(move.MoveData.AudioClip);

            float distance = 1;
            Vector3 origin = transform.localPosition;
            Vector3 target = transform.localPosition;
            if (_isplayer)
            {
                target += new Vector3(distance, 0);
            }
            else
            {
                target -= new Vector3(distance, 0);
            }
            float duration = 0.25f;

            while (transform.localPosition != target)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, distance * Time.unscaledDeltaTime / duration);
                yield return null;
            }
            
            while (transform.localPosition != origin)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, origin, distance * Time.unscaledDeltaTime / duration);
                yield return null;
            }

        }

        public IEnumerator HitRoutine()
        {
            //AudioManager.Instance.PlaySFX(_damagedclip);

            float duration = 0.25f;
            Color startcolor = _sr.color;

            float tick = 0;
            while (tick < 1)
            {
                tick += Time.unscaledDeltaTime / duration;
                _sr.color = Color.Lerp(startcolor, Color.gray, tick);
                yield return null;
            }

            while (tick > 0)
            {
                tick -= Time.unscaledDeltaTime / duration;
                _sr.color = Color.Lerp(startcolor, Color.gray, tick);
                yield return null;
            }
            
            //yield return _ui.HealthBar.UpdateRoutine(Pokemon.HP);
            yield return _healthbarelement.ChangeRoutine(_pokemon.HP);
        }
        public IEnumerator HealRoutine()
        {
            //yield return _ui.HealthBar.UpdateRoutine(Pokemon.HP);
            yield return _healthbarelement.ChangeRoutine(_pokemon.HP);
        }




        

        //private void OnMouseDown(MouseDownEvent mousedownevent)
        //{
        //    Debug.Log(mousedownevent);
        //}
        //private void OnMouseUp(MouseUpEvent mouseupevent)
        //{
        //    Debug.Log(mouseupevent);
        //}
        //private void OnMouseMove(MouseMoveEvent mousemoveevent)
        //{
        //    Debug.Log(mousemoveevent);
        //}
    }
}
