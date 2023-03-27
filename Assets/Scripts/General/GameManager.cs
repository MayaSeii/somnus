using System;
using Controllers;
using UnityEngine;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public PlayerController Player { get; private set; }
        public Camera MainCamera { get; private set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Game Manager in the scene.");
            Instance = this;
            
            Player = FindObjectOfType<PlayerController>();
            MainCamera = Camera.main;
        }
    }
}
