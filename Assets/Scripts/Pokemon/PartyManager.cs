using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PartyManager : MonoBehaviour
    {
        public static PartyManager Instance;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = new();
            }

            LoadParty();
        }

        private readonly PokemonSlot[] _slots = new PokemonSlot[6];
        public PokemonSlot[] Slots => _slots;
        public Party mystartingparty;

        private void LoadParty()
        {
            for (int i = 0; i < Slots.Length && i < mystartingparty.Pokemon.Length; i++)
            {
                Slots[i].Pokemon = new(mystartingparty.Pokemon[i]);
            }
        }



        public bool Add(Pokemon pokemon)
        {
            if (pokemon == null || pokemon.PokemonData == null)
            {
                return false;
            }
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Pokemon == null || Slots[i].Pokemon.PokemonData == null)
                {
                    Slots[i].Pokemon = pokemon;
                    return true;
                }
                
            }
            return false;
        }

        public void HealEveryone()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Pokemon pokemon = _slots[i].Pokemon;
                if (pokemon != null && pokemon.PokemonData != null)
                {
                    pokemon.Restore();
                }
            }
            Debug.Log("Party Healed");
        }

        public Pokemon GetFirstHealthyPokemon()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Pokemon.CanBattle(Slots[i].Pokemon))
                {
                    return Slots[i].Pokemon;
                }
            }

            return null;
        }

        public bool HasHealthyPokemon()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Pokemon.CanBattle(Slots[i].Pokemon))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
