using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _bar;
        [SerializeField] private Text _healthlabel;
        private float _max;
        private float _current;

        public void Initialize(float max)
        {
            _max = max;
        }

        public void Set(float current)
        {
            if (_max <= 0)
            {
                return;
            }
            _current = current;
            float percentage = _current / _max;
            if (percentage > 0.5f)
            {
                _bar.color = Color.green;
            }
            else if (percentage > 0.25f)
            {
                _bar.color = Color.yellow;
            }
            else
            {
                _bar.color = Color.red;
            }
            _bar.transform.localScale = new(percentage, 1);


            string currenthealth = string.Format("{0:0}", _current);
            string maxhealth = string.Format("{0:0}", _max);
            _healthlabel.text = $"{currenthealth}/{maxhealth}";

        }

        public IEnumerator ChangeRoutine(float target)
        {
            float distance = Mathf.Abs(_current - target);

            while (_current != target)
            {
                _current = Mathf.MoveTowards(_current, target, distance * Time.unscaledDeltaTime);
                Set(_current);
                yield return null;
            }
        }

        public void Hide()
        {
            _background.enabled = false;
            _bar.enabled = false;
            _healthlabel.enabled = false;
        }
        public void Show()
        {
            _background.enabled = true;
            _bar.enabled = true;
            _healthlabel.enabled = true;
        }
    }
}
