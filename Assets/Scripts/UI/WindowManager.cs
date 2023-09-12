using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance { get; private set; }
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

        private WindowManager_State _state;
        [SerializeField] private Area _currentarea;
        [SerializeField] private PauseWindow _pausewindowprefab;
        [SerializeField] private BattleWindow _battlewindowprefab;
        [SerializeField] private ControlsWindow _controlswindowprefab;

        private void Start()
        {
            SetState(new ShowControlsState(this));
        }

        //public void Pause()
        //{
        //    if (Time.timeScale == 0)
        //    {
        //        return;
        //    }
        //    Time.timeScale = 0;

        //    Window window = Instantiate(_pausewindowprefab);
        //    void OnClose()
        //    {
        //        Destroy(window.gameObject);
        //        Time.timeScale = 1;
        //    }
        //    window.Closed += OnClose;
        //}

        public void Pause()
        {
            if (_state is PlayState)
            {
                SetState(new PauseState(this));
            }
        }


        private class WindowManager_State
        {
            public virtual void Start() { }
            public virtual void End() { }
        }

        private void SetState(WindowManager_State state)
        {
            _state?.End();
            _state = state;
            _state?.Start();
        }

        private class PlayState : WindowManager_State
        {
            private readonly WindowManager _owner;

            public PlayState(WindowManager owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                Time.timeScale = 1;

                if (_owner._currentarea != null)
                {
                    _owner._currentarea.PlayMusic();
                }
            }

            public override void End()
            {
                Time.timeScale = 0;

                AudioManager.Instance.StopMusic();
            }
        }
        private class PauseState : WindowManager_State
        {
            private readonly WindowManager _owner;
            private PauseWindow _pausewindow;

            public PauseState(WindowManager owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                Time.timeScale = 0;

                _pausewindow = Instantiate(_owner._pausewindowprefab);
                _pausewindow.CloseClicked += () =>
                {
                    _owner.SetState(new PlayState(_owner));
                };
            }

            public override void End()
            {
                Destroy(_pausewindow.gameObject);
            }
        }

        public void StartBattle(Pokemon pokemon)
        {
            if (_state is PlayState)
            {
                StartCoroutine(LoadBattle(pokemon));
            }
        }
        private IEnumerator LoadBattle(Pokemon pokemon)
        {
            Time.timeScale = 0;
            yield return FadeController.Instance.FadeOut();
            SetState(new BattleState(this, pokemon));
            yield return FadeController.Instance.FadeIn();
        }

        private class BattleState : WindowManager_State
        {
            private readonly WindowManager _owner;
            private readonly Pokemon _pokemon;
            private BattleWindow _battlewindow;

            public BattleState(WindowManager owner, Pokemon pokemon)
            {
                _owner = owner;
                _pokemon = pokemon;
            }

            public override void Start()
            {
                Time.timeScale = 0;

                _battlewindow = Instantiate(_owner._battlewindowprefab);
                void OnBattleEnd()
                {
                    _owner.SetState(new PlayState(_owner));
                }
                _battlewindow.EndOfBattle += OnBattleEnd;
                
                _battlewindow.Initialize(_pokemon);
            }

            public override void End()
            {
                Destroy(_battlewindow.gameObject);
            }
        }



        private class ShowControlsState : WindowManager_State
        {
            private readonly WindowManager _owner;
            private ControlsWindow _controlswindow;

            public ShowControlsState(WindowManager owner)
            {
                _owner = owner;
            }

            public override void Start()
            {
                Time.timeScale = 0;

                _controlswindow = Instantiate(_owner._controlswindowprefab);
                _controlswindow.CloseClicked += () =>
                {
                    _owner.SetState(new PlayState(_owner));
                };
            }

            public override void End()
            {
                Destroy(_controlswindow.gameObject);
            }
        }










    }
}
