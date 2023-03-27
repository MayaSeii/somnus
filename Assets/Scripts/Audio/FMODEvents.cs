using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class FMODEvents : MonoBehaviour
    {
        public static FMODEvents Instance { get; private set; }
        
        [field: Header("Music")]
        [field: SerializeField] public EventReference GameStart { get; private set; }
        [field: SerializeField] public EventReference RandomMusic { get; private set; }
    
        [field: Header("Player")]
        [field: SerializeField] public EventReference Footsteps { get; private set; }
    
        [field: Header("Ambience")]
        [field: SerializeField] public EventReference LightHum { get; private set; }
        [field: SerializeField] public EventReference DeepHum { get; private set; }
    
        [field: Header("Clock")]
        [field: SerializeField] public EventReference ClockChime { get; private set; }
    
        [field: Header("Light Switch")]
        [field: SerializeField] public EventReference LightSwitchOn { get; private set; }
        [field: SerializeField] public EventReference LightSwitchOff { get; private set; }
    
        [field: Header("Door")]
        [field: SerializeField] public EventReference DoorOpen { get; private set; }
        [field: SerializeField] public EventReference DoorClose { get; private set; }
        [field: SerializeField] public EventReference StandDoorOpen { get; private set; }
        [field: SerializeField] public EventReference StandDoorClose { get; private set; }
    
        [field: Header("Paper Ball")]
        [field: SerializeField] public EventReference PaperBallGrab { get; private set; }
        [field: SerializeField] public EventReference PaperBallHit { get; private set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one FMOD Events instance in the scene.");
            Instance = this;
        }
    }
}
