using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class Character : MonoBehaviour
    {
        private Animator _animator;
        private MyInputActions _inputactions;
        public Vector2 InputDirection { get; private set; }
        public Vector2 Direction { get; set; }
        public bool IsBusy { get; set; }
        protected State _state;

        public float walkspeed = 4;
        public bool ismoving;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _inputactions = new();
            _inputactions.Player1.A.performed += InputA;
            _inputactions.Player1.B.performed += InputB;
            _inputactions.Player1.X.performed += InputX;
            _inputactions.Player1.Y.performed += InputY;
            _inputactions.Player1.Start.performed += InputStart;
        }

        public void UpdateAnimation()
        {
            if (_animator != null)
            {
                _animator.SetFloat("x", Direction.x);
                _animator.SetFloat("y", Direction.y);
                _animator.SetBool("ismoving", ismoving);
            }
        }

        private void Update()
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }

            InputDirection = _inputactions.Player1.Move.ReadValue<Vector2>();

            _state?.Update();

            UpdateAnimation();
        }
        /*private void FixedUpdate()
        {
            //myrb.MovePosition(myrb.position + myspeed * Time.fixedDeltaTime * mydirection);
        }*/

        #region input
        public void DisableInput()
        {
            _inputactions.Disable();
        }
        public void EnableInput()
        {
            _inputactions.Enable();
        }

        private void InputStart(InputAction.CallbackContext value)
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }
            _state?.StartPressed();
        }
        private void InputA(InputAction.CallbackContext value)
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }
            _state?.InputA();
        }
        private void InputB(InputAction.CallbackContext value)
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }
            _state?.InputB();
        }
        private void InputX(InputAction.CallbackContext value)
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }
            _state?.InputX();
        }
        private void InputY(InputAction.CallbackContext value)
        {
            if (Time.timeScale == 0 || IsBusy)
            {
                return;
            }
            _state?.InputY();
        }
        #endregion


        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.TryGetComponent(out Area area))
        //    {
        //        area.PlayMusic();
        //        //area.LoadScene();
        //    }
        //}
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.TryGetComponent(out Area area))
        //    {
        //        //area.UnloadScene();
        //    }
        //}

        public void Move(Vector2 direction, float speed)
        {
            if (ismoving)
            {
                return;
            }
            
            Direction = direction.Align();
            Vector3 targetposition = transform.position + (Vector3)Direction;

            if (IsWalkable(targetposition))
            {
                StartCoroutine(Co_Move(targetposition, speed));
            }
        }

        public IEnumerator Co_Move(Vector3 targetposition, float speed)
        {
            ismoving = true;

            while (transform.position != targetposition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetposition, speed * Time.deltaTime);
                yield return null;
            }
            
            /*Vector2 startposition = transform.position;
            Vector2 endposition = startposition + mymoveinput.AlignToAxis();
            float t = 0;
            while (t < 1)
            {
                t += walkspeed * Time.deltaTime;
                transform.position = Vector3.Lerp(startposition, endposition, t);
                yield return null;
            }*/
            CheckForEncounters();

            ismoving = false;
        }

        public bool IsWalkable(Vector3 targetposition)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(targetposition);
            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].isTrigger)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckForWater(Vector3 position)
        {
            Collider2D collider = Physics2D.OverlapPoint(position, LayerMask.GetMask("WaterTiles"));
            return collider != null;
        }

        public void CheckForEncounters()
        {
            Collider2D collider = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Encounters"));
            if (collider != null)
            {
                if (collider.TryGetComponent(out Encounters encounters))
                {
                    encounters.GetEncounter();
                }
            }
        }

        public void CastInteract()
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(transform.position + (Vector3)Direction);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(Direction);
                }
            }
        }

        public class State
        {
            public virtual void Start() { }
            public virtual void Update() { }
            public virtual void End() { }
            public virtual void Collision(Collision collision) { }
            //public virtual void OnTriggerEnter2D(Collider2D collision) { }
            public virtual void InputA() { }
            public virtual void InputB() { }
            public virtual void InputX() { }
            public virtual void InputY() { }
            public virtual void StartPressed() { }
        }

        protected void SetState(State state)
        {
            _state?.End();
            _state = state;
            _state?.Start();
        }
    }
}