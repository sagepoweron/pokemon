using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Cursor : MonoBehaviour
    {
        public void SetPosition(Vector3 _position)
        {
            transform.position = _position;
        }

        public void Interact()
        {
            /*Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);
            if (hit != null)
            {
                WorldObject worldobject = hit.GetComponent<WorldObject>();
                worldobject?.OnCut();

                //ItemDrop itemdrop = hit.GetComponent<ItemDrop>();
                //itemdrop?.Collect();
            }*/


            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

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
    }
}