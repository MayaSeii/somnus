using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class FMODEvents : MonoBehaviour
    {
        public static FMODEvents Instance { get; private set; }
        
        [field: Header("Music")]
        [field: SerializeField] public EventReference TitlePiano { get; private set; }
        [field: SerializeField] public EventReference GameStart { get; private set; }
        [field: SerializeField] public EventReference RandomMusic { get; private set; }
        [field: SerializeField] public EventReference KeySmash { get; private set; }
        
        [field: Header("Interface")]
        [field: SerializeField] public EventReference MenuBuzz { get; private set; }
        [field: SerializeField] public EventReference MenuExit { get; private set; }
        [field: SerializeField] public EventReference PageButton { get; private set; }
        [field: SerializeField] public EventReference SettingsButton { get; private set; }
        [field: SerializeField] public EventReference RebindRepeat { get; private set; }
        [field: SerializeField] public EventReference SliderTick { get; private set; }
        [field: SerializeField] public EventReference Scrollbar { get; private set; }
        [field: SerializeField] public EventReference Hover { get; private set; }
    
        [field: Header("Player")]
        [field: SerializeField] public EventReference Footsteps { get; private set; }
    
        [field: Header("Ambience")]
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
            Instance = this;
        }
    }
}
