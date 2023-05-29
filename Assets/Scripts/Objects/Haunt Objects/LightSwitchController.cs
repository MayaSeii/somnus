using Audio;
using Haunts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightController
{
    public class LightSwitchController : MonoBehaviour
    {
        public static LightSwitchController Instance;

        public int ActiveSwitches;
        [HideInInspector] public bool AllLightsOff;

        private float _timer;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            _timer = HauntManager.Instance.HauntTimer;
            ActiveSwitches = 5;
        }

        private void Update()
        {
            if (AllLightsOff) _timer -= Time.deltaTime;
            if(AllLightsOff && _timer <= 0 && HauntManager.Instance.FatherInstance == null)
            {
                _timer = HauntManager.Instance.HauntTimer;
                AllLightsOff = false;
                HauntManager.Instance.ForceSpawnFather(default);
            }
            if(AllLightsOff && ActiveSwitches >= 3)
            {
                AudioManager.PlayOneShot(FMODEvents.Instance.KeySmash, transform.position);
                _timer = HauntManager.Instance.HauntTimer;
                AllLightsOff = false;
            }
        }
    }
}
