using General;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Haunts
{
    public class HauntManager : MonoBehaviour
    {
        public static HauntManager Instance { get; set; }
        
        [field: Header("Nightmares")]
        [field: SerializeField] public GameObject Father { get; set; }
        [field: SerializeField] public GameObject Mother { get; set; }
        [field: SerializeField] public GameObject Daughter { get; set; }

        private GameObject _fatherInstance;
        private GameObject _motherInstance;
        private GameObject _daughterInstance;
        
        [field: Header("Haunts")]
        [field: SerializeField] public LightsOffHaunt LightsOffHaunt { get; set; }
        [field: SerializeField] public ClockChimeHaunt ClockChimeHaunt { get; set; }
        [field: SerializeField] public TVOnHaunt TVOnHaunt { get; set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Haunt Manager in the scene.");
            Instance = this;
        }

        public void ForceSpawnFather(InputAction.CallbackContext context)
        {
            if (_fatherInstance == null)
            {
                _fatherInstance = Instantiate(Father, new Vector3(0f, -0.145f, 12f), Quaternion.Euler(0f, 180f, 0f));
            }
            else
            {
                Destroy(_fatherInstance);
                _fatherInstance = null;
            }
        }
        
        public void ForceSpawnMother(InputAction.CallbackContext context)
        {
            if (_motherInstance == null)
            {
                _motherInstance = Instantiate(Mother, new Vector3(-6f, -0.145f, 8.5f), Quaternion.identity);
            }
            else
            {
                Destroy(_motherInstance);
                _motherInstance = null;
            }
        }
        
        public void ForceSpawnDaughter(InputAction.CallbackContext context)
        {
            if (_daughterInstance == null)
            {
                _daughterInstance = Instantiate(Daughter, new Vector3(-13f, -0.145f, -5f), Quaternion.identity);
            }
            else
            {
                Destroy(_daughterInstance);
                _daughterInstance = null;
            }
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
        
        public void ForceTVOnHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(TVOnHaunt);
            DebugManager.Instance.UpdateLastHaunt("TV On (F)");
        }

        private static void ExecuteHaunt(Haunt haunt)
        {
            haunt.Execute();
        }
    }
}
