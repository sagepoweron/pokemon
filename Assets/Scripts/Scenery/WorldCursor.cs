using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
	public class WorldCursor : MonoBehaviour
	{
		public LayerMask layermask;
		public Transform target;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			Vector3 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			transform.position = new Vector3(position.x, position.y, 0);

			if (Mouse.current.leftButton.wasPressedThisFrame)
			{
				GrabTarget();
			}

			if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
				DropTarget();
            }

			if (target != null)
			{
				target.localPosition = Vector3.zero;
			}
		}

		public void GrabTarget()
        {
			if (target == null)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, layermask);

				if (hit.collider != null)
				{
					target = hit.collider.transform;
					target.parent = transform;
				}
			}
		}

		public void DropTarget()
        {
			if (target != null)
			{
				transform.DetachChildren();
				target = null;
			}
		}
	}
}