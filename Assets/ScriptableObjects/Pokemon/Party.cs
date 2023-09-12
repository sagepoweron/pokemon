using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "ScriptableObject/Party")]
    public class Party : ScriptableObject
    {
        [SerializeField] private Pokemon[] _pokemon = new Pokemon[6];
        public Pokemon[] Pokemon => _pokemon;

    }
}
