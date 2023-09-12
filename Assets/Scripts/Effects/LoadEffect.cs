using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class LoadEffect : MonoBehaviour
    {
        [SerializeField] private Image _fadeimage;
        [SerializeField] private Image _circleimage;
        [SerializeField] private Image _fillimage;

        [SerializeField] private float _transitiontime = 0.5f;

        public static LoadEffect Instance { get; set; }
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

        public void Load(Action load)
        {
            StartCoroutine(CircleRoutine(load));
        }

        public IEnumerator FadeOut()
        {
            float t = 0;
            Color color = new(0, 0, 0, 0);
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / _transitiontime;
                color.a = t;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
        }

        public IEnumerator FadeIn()
        {
            float t = 1;
            Color color = new(0, 0, 0, 1);
            while (t > 0)
            {
                t -= Time.unscaledDeltaTime / _transitiontime;
                color.a = t;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
        }

        private IEnumerator FadeRoutine(Action load)
        {
            _fadeimage.enabled = true;
            //Time.timeScale = 0;

            float t = 0;
            Color color = new(0, 0, 0, 0);
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / _transitiontime;
                color.a = t;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
            
            load?.Invoke();

            /*AsyncOperation asyncloadscene = SceneManager.LoadSceneAsync(sceneinfo.myscene);
            while (!asyncloadscene.isDone)
            {
                yield return null;
            }

            AsyncOperation asyncloadsubscene = SceneManager.LoadSceneAsync("ui", LoadSceneMode.Additive);
            // Wait until the asynchronous scene fully loads
            while (!asyncloadsubscene.isDone)
            {
                yield return null;
            }*/

            //Time.timeScale = 1;

            while (t > 0)
            {
                t -= Time.unscaledDeltaTime / _transitiontime;
                color.a = t;// Mathf.Lerp(0, 1, t);
                _fadeimage.color = color;
                yield return null;
            }
            _fadeimage.enabled = false;
        }

        public IEnumerator CircleRoutine(Action load)
        {
            _circleimage.enabled = true;
            float t = 0;
            float size = 1000;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / _transitiontime;
                size = Mathf.Lerp(1000, 0, t);
                _circleimage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                yield return null;
            }

            load?.Invoke();

            while (t > 0)
            {
                t -= Time.unscaledDeltaTime / _transitiontime;
                size = Mathf.Lerp(1000, 0, t);
                _circleimage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                yield return null;
            }
            _circleimage.enabled = false;
        }

        private IEnumerator FillRoutine(Action load)
        {
            _fillimage.enabled = true;
            float t = 0;
            _fillimage.fillAmount = 0;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / _transitiontime;
                _fillimage.fillAmount = t;
                yield return null;
            }

            load?.Invoke();

            while (t > 0)
            {
                t -= Time.unscaledDeltaTime / _transitiontime;
                _fillimage.fillAmount = t;
                yield return null;
            }
            _fillimage.enabled = false;
        }
    }
}
