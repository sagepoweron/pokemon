using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/Pokeball")]
    public class Pokeball : ItemData
    {
        [SerializeField] private float _catchrate = 1;
        public float CatchRate => _catchrate;
    }
}
