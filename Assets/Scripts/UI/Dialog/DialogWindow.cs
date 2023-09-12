using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class DialogWindow : Window
    {
        [SerializeField] private Text _dialoglabel;
        [SerializeField] private Button _nextbutton;
        [SerializeField] private float _letterspersecond = 40;
        private bool _continue;

        private void OnEnable()
        {
            _nextbutton.onClick.AddListener(Next);

			_nextbutton.Select();
		}

		public void ShowLine(string line)
        {
            StartCoroutine(ShowLineRoutine(line));
        }

        public IEnumerator ShowLineRoutine(string line)
        {
            _dialoglabel.text = "";
            char[] letters = line.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                _dialoglabel.text += letters[i];

                yield return new WaitForSecondsRealtime(1 / _letterspersecond);
            }

            _continue = false;
            yield return new WaitUntil(() => _continue);
        }

        public IEnumerator ShowDialog(List<string> lines)
        {
            for (int line = 0; line < lines.Count; line++)
            {
                _dialoglabel.text = "";
                char[] letters = lines[line].ToCharArray();
                for (int i = 0; i < letters.Length; i++)
                {
                    _dialoglabel.text += letters[i];

                    yield return new WaitForSecondsRealtime(1 / _letterspersecond);
                }

                _continue = false;
                yield return new WaitUntil(() => _continue);
            }
        }


        public void Next()
        {
            _continue = true;
        }
    }
}
