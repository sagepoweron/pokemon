using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyGame
{
    public class Prefabs : MonoBehaviour
    {
        public static Prefabs Instance { get; set; }
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
        }

        [Header("Prefabs")]
        public DialogWindow _dialogwindowprefab;

        public BattleWindow _battleprefab;
    }
}