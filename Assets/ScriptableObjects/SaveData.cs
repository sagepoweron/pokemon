using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "SaveData")]
    public class SaveData : ScriptableObject//, ISerializationCallbackReceiver
    {
        public int hp;
        public int mp;
        public int attack;
        public int defense;

        //public void OnBeforeSerialize() { }
        //public void OnAfterDeserialize() { }
    }
}