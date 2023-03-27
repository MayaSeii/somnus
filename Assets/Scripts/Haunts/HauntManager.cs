using General;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Haunts
{
    public class HauntManager : MonoBehaviour
    {
        public static HauntManager Instance { get; set; }
        
        [field: Header("Haunts")]
        [field: SerializeField] public LightsOffHaunt LightsOffHaunt { get; set; }
        [field: SerializeField] public ClockChimeHaunt ClockChimeHaunt { get; set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Haunt Manager in the scene.");
            Instance = this;
        }
        public void ForceLightsOffHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(LightsOffHaunt);
            DebugManager.Instance.UpdateLastHaunt("Lights Out (F)");
        }

        public void ForceClockChimeHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(ClockChimeHaunt);
            DebugManager.Instance.UpdateLastHaunt("Clock Chime (F)");
        }

        private static void ExecuteHaunt(Haunt haunt)
        {
            haunt.Execute();
        }
    }
}
