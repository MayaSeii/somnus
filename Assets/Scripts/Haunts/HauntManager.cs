using Audio;
using General;
using UI;
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

        private GameObject _fatherInstance;
        private GameObject _motherInstance;
        private GameObject _daughterInstance;

        [field: Header("Haunts")]
        [field: SerializeField] public float HauntTimer { get; set; }
        [field: SerializeField] public LightsOffHaunt LightsOffHaunt { get; set; }
        [field: SerializeField] public ClockChimeHaunt ClockChimeHaunt { get; set; }
        [field: SerializeField] public TVOnHaunt TVOnHaunt { get; set; }
        [field: SerializeField] public KitchenPhoneHaunt KitchenPhoneHaunt { get; set; }

        private List<Haunt> _fatherhaunts;
        private List<Haunt> _motherhaunts;
        private List<Haunt> _daughterhaunts;
        private List<Haunt> _dreamhaunts;
        private float _timer;

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Haunt Manager in the scene.");
            Instance = this;

            _fatherhaunts = new List<Haunt>();
            _fatherhaunts.Add(LightsOffHaunt);

            _motherhaunts = new List<Haunt>();
            _motherhaunts.Add(KitchenPhoneHaunt);

            _daughterhaunts = new List<Haunt>();
            _daughterhaunts.Add(TVOnHaunt);

            _dreamhaunts = new List<Haunt>();
            _dreamhaunts.Add(ClockChimeHaunt);

        }

        private void Start()
        {
            _timer = HauntTimer;
            StartCoroutine(ActivateFatherHauntCoroutine());
            StartCoroutine(ActivateMotherHauntCoroutine());
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0) _timer = HauntTimer;

            int roundedTimer = Mathf.RoundToInt(_timer);
            DebugManager.Instance.UpdateNextHaunt(roundedTimer);

            /*if (_fatherInstance)
            {
                ForceSpawnFather(default);
            }

            if (_motherInstance)
            {
                ForceSpawnMother(default);
            }

            if (_daughterInstance != null)
            {
                new WaitForSeconds(3f);
                ForceSpawnDaughter(default);
            }*/
        }

        public void ForceSpawnFather(InputAction.CallbackContext context)
        {
            if (_fatherInstance == null)
            {
                _fatherInstance = Instantiate(Father, new Vector3(0f, -0.145f, 12f), Quaternion.Euler(0f, 180f, 0f));
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);
            }
            else
            {
                Destroy(_fatherInstance);
                _fatherInstance = null;
                AudioManager.Instance.StopAmbience("Father Music");
            }
        }
        
        public void ForceSpawnMother(InputAction.CallbackContext context)
        {
            if (_motherInstance == null)
            {
                _motherInstance = Instantiate(Mother, new Vector3(-6f, -0.145f, 8.5f), Quaternion.identity);
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);
            }
            else
            {
                Destroy(_motherInstance);
                _motherInstance = null;
                AudioManager.Instance.StopAmbience("Father Music");
            }
        }
        
        public void ForceSpawnDaughter(InputAction.CallbackContext context)
        {
            if (_daughterInstance == null)
            {
                _daughterInstance = Instantiate(Daughter, new Vector3(-13f, -0.145f, -5f), Quaternion.identity);
                AudioManager.Instance.InitialiseAmbience("Father Music", FMODEvents.Instance.FatherMusic, GameManager.Instance.Player.transform);
            }
            else
            {
                Destroy(_daughterInstance);
                _daughterInstance = null;
                AudioManager.Instance.StopAmbience("Father Music");
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

        private static void ExecuteHaunt(Haunt haunt)
        {
            haunt.Execute();
        }
        public IEnumerator ActivateFatherHauntCoroutine()
        {
            yield return new WaitForSeconds(5f);
            while (true)
            {
                int fatherIndex = Random.Range(0, _fatherhaunts.Count);

                Haunt fatherHaunt = _fatherhaunts[fatherIndex];
                fatherHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer);
            }
        }

        public IEnumerator ActivateMotherHauntCoroutine()
        {
            yield return new WaitForSeconds(HauntTimer);
            while (true)
            {
                int motherIndex = Random.Range(0, _motherhaunts.Count);

                Haunt motherHaunt = _motherhaunts[motherIndex];
                motherHaunt.Execute();
                yield return new WaitForSeconds(HauntTimer * 2);
            }
        }
        public IEnumerator ActivateDaughterHauntCoroutine()
        {
            while (true)
            {
                int daughterIndex = Random.Range(0, _daughterhaunts.Count);

                Haunt daughterHaunt = _daughterhaunts[daughterIndex];
                daughterHaunt.Execute();
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
    }
}
