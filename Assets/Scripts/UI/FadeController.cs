using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private float _transitiontime = 0.5f;
        [SerializeField] private Image _fadeimage;

        public static FadeController Instance { get; set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public IEnumerator FadeOut()
        {
            Color color = Color.black;
            color.a = 0;
            while (color.a < 1)
            {
                color.a += Time.unscaledDeltaTime / _transitiontime;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            Color color = Color.black;
            color.a = 1;
            while (color.a > 0)
            {
                color.a -= Time.unscaledDeltaTime / _transitiontime;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
        }

        private IEnumerator FadeRoutine(Action action)
        {
            Color color = Color.black;
            color.a = 0;
            while (color.a < 1)
            {
                color.a += Time.unscaledDeltaTime / _transitiontime;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }

            action?.Invoke();

            while (color.a > 0)
            {
                color.a -= Time.unscaledDeltaTime / _transitiontime;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
        }
    }
}
