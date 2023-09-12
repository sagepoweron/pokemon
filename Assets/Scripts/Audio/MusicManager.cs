using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public List<AudioClip> audioclips;
        public int _index;

        [SerializeField] private Text _songtitletext;
        [SerializeField] private Text _songtimetext;

        private int musiclength;
        private int playtime;
        private int minutes;
        private int seconds;
        

        private AudioSource audiosource;

        public string path;

        // Start is called before the first frame update
        void Start()
        {
            audiosource = GetComponent<AudioSource>();

            Play();
        }

        private void Update()
        {
            if (audiosource.isPlaying)
            {
                ShowTime();
            }
        }

        /*public void LoadSong()
        {
            StartCoroutine(GetAudioClip());
        }

        IEnumerator GetAudioClip()
        {
            string url = string.Format("file://{0}", path);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    audiosource.clip = DownloadHandlerAudioClip.GetContent(www);
                    text_musictitle.text = audiosource.clip.name;
                }
            }
        }*/


        public void Play()
        {
            //StopAllCoroutines();
            audiosource.Stop();
            audiosource.clip = audioclips[_index];
            audiosource.Play();
            //text_musictitle.text = audiosource.clip.name;
            _songtitletext.text = $"{_index + 1}. {audiosource.clip.name}";
            //StartCoroutine(WaitForMusicEnd());
        }
        public void PlayPause()
        {
            if (!audiosource.isPlaying)
            {
                if (audiosource.time == 0)
                {
                    audiosource.clip = audioclips[_index];
                    audiosource.Play();
                    _songtitletext.text = $"{_index + 1}. {audiosource.clip.name}";
                    //StartCoroutine(WaitForMusicEnd());
                }
                else
                {
                    audiosource.UnPause();
                    //StartCoroutine(WaitForMusicEnd());
                }
            }
            else
            {
                //StopAllCoroutines();
                audiosource.Pause();
            }
        }

        public void Next()
        {
            //_index = (_index + 1) % audioclips.Count;
            _index += 1;
            if (_index > audioclips.Count - 1)
            {
                _index = 0;
            }

            Play();
        }
        public void Back()
        {
            _index -= 1;
            if (_index < 0)
            {
                _index = audioclips.Count - 1;
            }

            Play();
        }
        public void Pause()
        {
            if (audiosource.isPlaying)
            {
                //StopAllCoroutines();
                audiosource.Pause();
            }
            else
            {
                if (audiosource.time != 0)
                {
                    audiosource.UnPause();
                    //StartCoroutine(WaitForMusicEnd());
                }
            }
            
        }
        public void Stop()
        {
            //StopAllCoroutines();
            audiosource.Stop();
            ShowTime();
        }

        /*IEnumerator WaitForMusicEnd()
        {
            while(audiosource.isPlaying)
            {
                ShowTime();
                yield return null;
            }
            Next();
        }*/
        public void Mute()
        {
            audiosource.mute = !audiosource.mute;
        }

        public void ShowTime()
        {
            musiclength = (int)audiosource.clip.length;
            playtime = (int)audiosource.time;

            seconds = playtime % 60;
            minutes = (playtime / 60) % 60;

            _songtimetext.text = string.Format("{0}:{1}/{2}:{3}", minutes, seconds.ToString("00"), (musiclength / 60) % 60, (musiclength % 60).ToString("00"));
        }



        public float _targetvolume = 0.5f;
        public float _fadeduration = 1;
        //private bool _crossfadeinprogress;
        public void CrossFade(AudioSource audiosource1, AudioSource audiosource2)
        {
            StopAllCoroutines();
            StartCoroutine(CrossFadeAsync(audiosource1, audiosource2));
        }
        private IEnumerator CrossFadeAsync(AudioSource audiosource1, AudioSource audiosource2)
        {
            //_crossfadeinprogress = true;
            while (audiosource1.volume > 0 && audiosource2.volume < _targetvolume)
            {
                if (audiosource1.volume > 0)
                {
                    audiosource1.volume -= _targetvolume * Time.unscaledDeltaTime / _fadeduration;
                }

                if (audiosource2.volume < _targetvolume)
                {
                    audiosource2.volume += _targetvolume * Time.unscaledDeltaTime / _fadeduration;
                }

                yield return null;
            }
            //_crossfadeinprogress = false;
        }
    }
}