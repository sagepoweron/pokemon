using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame
{
    public class Encounters : MonoBehaviour
    {
        [SerializeField] private List<PokemonEncounter> _encounters = new();

        public void GetEncounter()
        {
            List<PokemonEncounter> encounters = _encounters.FindAll((encounter) => encounter.HasPokemon());
            if (encounters.Count > 0)
            {
                PokemonEncounter encounter = encounters[Random.Range(0, encounters.Count)];

                WindowManager.Instance.StartBattle(encounter.GetPokemon);
                
            }
        }
    }
}
