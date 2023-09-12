using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class WorldCamera : MonoBehaviour
    {
        public Transform Target;
        public Vector3 offset;
        public float zoom;
        //public float speed;

        Vector2 size;
        Vector2 distance;
        Vector3 targetposition;

        // Start is called before the first frame update
        void Start()
        {
            float ratio = (float)UnityEngine.Screen.width / (float)UnityEngine.Screen.height;
            size = new Vector2(Camera.main.orthographicSize * ratio, Camera.main.orthographicSize);
        }

        /*private void Update()
        {
            if (InputManager.Instance.gamepad1.XPressed)
            {
                zoom = Mathf.Clamp(zoom - 0.1f, 1, 2);
            }

            if (InputManager.Instance.gamepad1.YPressed)
            {
                zoom = Mathf.Clamp(zoom + 0.1f, 1, 2);
            }
        }*/

        private void LateUpdate()
        {
            if (Target != null)
            {
                transform.position = Target.position + offset;
                //transform.position = Vector3.MoveTowards(transform.position, Target.position + offset, speed * Time.deltaTime);

                /*Camera.main.orthographicSize = size.y / zoom;

                distance = Target.position - transform.position;

                Vector2 bounds = size / 2 / zoom;
                if (Mathf.Abs(distance.x) > bounds.x)
                {
                    targetposition.x = Target.position.x - (bounds.x * Mathf.Sign(distance.x));
                }
                if (Mathf.Abs(distance.y) > bounds.y)
                {
                    targetposition.y = Target.position.y - (bounds.y * Mathf.Sign(distance.y));
                }
                transform.position = targetposition + offset;*/
                
            }
            
        }
    }
}