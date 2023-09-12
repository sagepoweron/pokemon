using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class GridCursor : MonoBehaviour
    {
        public Grid grid;

        Vector3 mouseposition;
        Vector3Int cellposition;

        public IStatus status;

        // Update is called once per frame
        void Update()
        {
            mouseposition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            cellposition = grid.WorldToCell(mouseposition);
            transform.position = grid.GetCellCenterWorld(cellposition);
            /*
            if (InputManager.Instance.mouse.leftButton.wasPressedThisFrame)
            {
                Click();
            }
            if (InputManager.Instance.mouse.rightButton.wasPressedThisFrame)
            {
                RightClick();
            }*/
        }

        public void Click()
        {
            /*Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);
            if (hit != null)
            {
                WorldObject worldobject = hit.GetComponent<WorldObject>();
                worldobject?.OnCut();

                //ItemDrop itemdrop = hit.GetComponent<ItemDrop>();
                //itemdrop?.Collect();
            }*/

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.25f);

            for (int i = 0; i < hits.Length; i++)
            {
                IChoppable choppable = hits[i].GetComponent<IChoppable>();
                if (choppable != null)
                {
                    choppable.Chop(1);
                }
                //ItemDrop itemdrop = hits[i].GetComponent<ItemDrop>();
                //itemdrop?.Collect();
            }
            
        }

        public void RightClick()
        {
            if (status != null)
            {
                status.Hide();
            }

            Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
            for (int i = 0; i < hits.Length; i++)
            {
                status = hits[i].GetComponent<IStatus>();
                if (status != null)
                {
                    status.Show();
                    break;
                }
            }
        }
    }
}