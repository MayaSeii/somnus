using Controllers;
using Inputs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public PlayerController Player { get; private set; }
        public Camera MainCamera { get; private set; }
        public Camera GameplayUICamera { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void InitialiseInMenu()
        {
            MainCamera = Camera.main;
        }

        public void InitialiseInGame()
        {
            Player = FindObjectOfType<PlayerController>();
            MainCamera = Camera.main;
            GameplayUICamera = GameObject.FindWithTag("Gameplay UI").GetComponent<Canvas>().worldCamera;
        }

        public static void ReturnToMenu()
        {
            ControlsManager.Instance.Unregister();
            SceneManager.LoadScene(1);
        }
    }
}
