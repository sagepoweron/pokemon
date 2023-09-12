using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    [System.Serializable]
    public class Indexer
    {
        public int mysize;
        public int index;

        public Indexer(int size)
        {
            mysize = size;
        }

        public void Up()
        {
            index += 1;
            if (index >= mysize)
            {
                index = 0;
            }
        }
        public void Down()
        {
            index -= 1;
            if (index < 0)
            {
                index = mysize - 1;
            }
        }


    }
}