using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private void Awake()
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

        [SerializeField] AudioSource _musicplayer;
        [SerializeField] AudioSource _sfxplayer;
        public float _targetvolume = 0.2f;
        public float _fadeduration = 1;

        public void PlayMusic(AudioClip audioclip, bool loop = true, bool fade = false)
        {
            if (audioclip == null)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(PlayMusicAsync(audioclip, loop, fade));
        }
        private IEnumerator PlayMusicAsync(AudioClip audioclip, bool loop, bool fade)
        {
            if (fade)
            {
                while (_musicplayer.volume > 0 && _fadeduration > 0)
                {
                    _musicplayer.volume -= _targetvolume * Time.unscaledDeltaTime / _fadeduration;
                    yield return null;
                }
            }
            _musicplayer.volume = 0;
            
            _musicplayer.clip = audioclip;
            _musicplayer.loop = loop;
            _musicplayer.Play();

            if (fade)
            {
                while (_musicplayer.volume < _targetvolume && _fadeduration > 0)
                {
                    _musicplayer.volume += _targetvolume * Time.unscaledDeltaTime / _fadeduration;
                    yield return null;
                }
            }
            _musicplayer.volume = _targetvolume;
        }
        public void StopMusic(bool fade = false)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicAsync(fade));
        }
        private IEnumerator StopMusicAsync(bool fade)
        {
            if (fade)
            {
                while (_musicplayer.volume > 0 && _fadeduration > 0)
                {
                    _musicplayer.volume -= _targetvolume * Time.unscaledDeltaTime / _fadeduration;
                    yield return null;
                }
            }
            _musicplayer.volume = 0;

            _musicplayer.Stop();
        }






        public void PlaySFX(AudioClip audioclip)
        {
            if (audioclip == null)
            {
                return;
            }

            _sfxplayer.PlayOneShot(audioclip);


        }
    }
}
