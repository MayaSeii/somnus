using Audio;
using General;
using UI;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

namespace Haunts
{
    public class HauntManager : MonoBehaviour
    {
        public static HauntManager Instance { get; set; }
        
        [field: Header("Nightmares")]
        [field: SerializeField] public GameObject Father { get; set; }
        [field: SerializeField] public GameObject Mother { get; set; }
        [field: SerializeField] public GameObject Daughter { get; set; }

        [HideInInspector] public GameObject FatherInstance;
        [HideInInspector] public GameObject MotherInstance;
        [HideInInspector] public GameObject DaughterInstance;

        [field: Header("Haunts")]
        [field: SerializeField] public float HauntTimer { get; set; }
        [field: SerializeField] public LightsOffHaunt LightsOffHaunt { get; set; }
        [field: SerializeField] public ClockChimeHaunt ClockChimeHaunt { get; set; }
        [field: SerializeField] public TVOnHaunt TVOnHaunt { get; set; }
        [field: SerializeField] public KitchenPhoneHaunt KitchenPhoneHaunt { get; set; }
        [field: SerializeField] public FrontDoorHaunt FrontDoorHaunt { get; set; }
        [field: SerializeField] public BathroomFatherHaunt BathroomFatherHaunt { get; set; }

        [HideInInspector] public bool HauntActive;
        private List<Haunt> _fatherhaunts;
        private List<Haunt> _motherhaunts;
        private List<Haunt> _daughterhaunts;
        private List<Haunt> _dreamhaunts;
        private bool _canStart;
        private float _timer;

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Haunt Manager in the scene.");
            Instance = this;

            _fatherhaunts = new List<Haunt>();
            _fatherhaunts.Add(LightsOffHaunt);
            _fatherhaunts.Add(BathroomFatherHaunt);

            _motherhaunts = new List<Haunt>();
            _motherhaunts.Add(KitchenPhoneHaunt);
            _motherhaunts.Add(FrontDoorHaunt);

            _daughterhaunts = new List<Haunt>();
            _daughterhaunts.Add(TVOnHaunt);

            _dreamhaunts = new List<Haunt>();
            _dreamhaunts.Add(ClockChimeHaunt);

            _canStart = true;
        }

        private void Start()
        {
            _timer = HauntTimer;
            StartAllHaunts(default);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0) _timer = HauntTimer;

            int roundedTimer = Mathf.RoundToInt(_timer);
            DebugManager.Instance.UpdateNextHaunt(roundedTimer);
        }

        public void ForceSpawnFather(InputAction.CallbackContext context)
        {
            if (FatherInstance == null)
            {
                FatherInstance = Instantiate(Father, new Vector3(0f, -0.145f, 12f), Quaternion.Euler(0f, 180f, 0f));
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);

                StartCoroutine(StopNightmare(FatherInstance, 30f));
                StopAllHaunts(default);
            }
            else
            {
                StartCoroutine(StopNightmare(FatherInstance, 0f));
            }
        }
        
        public void ForceSpawnMother(InputAction.CallbackContext context)
        {
            if (MotherInstance == null)
            {
                MotherInstance = Instantiate(Mother, new Vector3(-6f, -0.145f, 8.5f), Quaternion.identity);
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);

                StartCoroutine(StopNightmare(MotherInstance, 30f));
                StopAllHaunts(default);
            }
            else
            {
                StartCoroutine(StopNightmare(MotherInstance, 0f));
            }
        }
        
        public void ForceSpawnDaughter(InputAction.CallbackContext context)
        {
            if (DaughterInstance == null)
            {
                DaughterInstance = Instantiate(Daughter, new Vector3(-13f, -0.145f, -5f), Quaternion.identity);
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);

                StartCoroutine(StopNightmare(DaughterInstance, 30f));
                StopAllHaunts(default);
            }
            else
            {
                StartCoroutine(StopNightmare(DaughterInstance, 0f));
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
        
        public void ForceKitchenPhoneHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(KitchenPhoneHaunt);
            DebugManager.Instance.UpdateLastHaunt("Kitchen Phone (F)");
        }
        public void ForceFrontDoorHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(FrontDoorHaunt);
            DebugManager.Instance.UpdateLastHaunt("Front Door (F)");
        }
        public void ForceBathroomFatherHaunt(InputAction.CallbackContext context)
        {
            ExecuteHaunt(BathroomFatherHaunt);
            DebugManager.Instance.UpdateLastHaunt("Bathroom Father (F)");
        }

        private static void ExecuteHaunt(Haunt haunt)
        {
            haunt.Execute();
        }
        public void StartAllHaunts(InputAction.CallbackContext context)
        {
            _canStart = true;
            StartCoroutine(ActivateFatherHauntCoroutine());
            StartCoroutine(ActivateMotherHauntCoroutine());
            if (PlayerController.Instance.RestAchieved >= 14) StartCoroutine(ActivateDaughterHauntCoroutine());
        }
        public void StopAllHaunts(InputAction.CallbackContext context)
        {
            _canStart = false;
            HauntActive = false;
            StopCoroutine(ActivateFatherHauntCoroutine());
            StopCoroutine(ActivateMotherHauntCoroutine());
            if (PlayerController.Instance.RestAchieved >= 14) StopCoroutine(ActivateDaughterHauntCoroutine());
        }
        public IEnumerator ActivateFatherHauntCoroutine()
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                int fatherIndex = Random.Range(0, _fatherhaunts.Count);

                Haunt fatherHaunt = _fatherhaunts[fatherIndex];
                if (!_canStart) yield break;
                else fatherHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer * 2);
            }
        }

        public IEnumerator ActivateMotherHauntCoroutine()
        {
            yield return new WaitForSeconds(9f);
            while (true)
            {
                int motherIndex = Random.Range(0, _motherhaunts.Count);

                Haunt motherHaunt = _motherhaunts[motherIndex];
                if (!_canStart) yield break;
                else motherHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer * 2);
            }
        }
        public IEnumerator ActivateDaughterHauntCoroutine()
        {
            yield return new WaitForSeconds(6f);
            while (true)
            {
                int daughterIndex = Random.Range(0, _daughterhaunts.Count);

                Haunt daughterHaunt = _daughterhaunts[daughterIndex];
                if (!_canStart) yield break;
                else daughterHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer * 2);
            }
        }
        public IEnumerator ActivateDreamHauntCoroutine()
        {
            while (true)
            {
                int dreamIndex = Random.Range(0, _dreamhaunts.Count);

                Haunt dreamHaunt = _dreamhaunts[dreamIndex];
                dreamHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer);
            }
        }
        public IEnumerator StopNightmare(GameObject nightmare, float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(nightmare);
            FatherInstance = null;
            MotherInstance = null;
            DaughterInstance = null;
            AudioManager.Instance.StopAmbience("Father Music");
            StartAllHaunts(default);
        }
    }
}
