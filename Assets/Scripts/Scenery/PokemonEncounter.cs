using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [System.Serializable]
    public class PokemonEncounter
    {
        [SerializeField] private PokemonData _pokemondata;
        [SerializeField] private int _levelmin;
        [SerializeField] private int _levelmax;
        [SerializeField] private float _rarity;

        //Rarity of Pokemon x
        //Very Common                   10
        //Common                        8.5
        //Semi-rare                     6.75
        //Rare                          3.33
        //Very Rare                     1.25
        

        public bool HasPokemon()
        {
            float encounterrate = _rarity / 187.5f;
            return Random.value <= encounterrate;
        }

        public Pokemon GetPokemon => new(_pokemondata, Random.Range(_levelmin, _levelmax) + 1);

        
    }
}
