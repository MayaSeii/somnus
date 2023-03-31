using Audio;
using General;
using Inputs;
using Settings;
using UnityEngine;

public class Initialiser : MonoBehaviour
{
    private void Awake()
    {
        if (!GameManager.Instance) return;
        
        GameManager.Instance.InitialiseInGame();
        ControlsManager.Instance.InitialiseInGame();
        AudioManager.Instance.InitialiseInGame();
        Config.ResetConfigs();
    }
}
