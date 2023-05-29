using System.Collections;
using Audio;
using Haunts;
using UnityEngine;

namespace Objects
{
    public class Television : Interactable
    {
        [field: SerializeField] public GameObject On { get; set; }
        [field: SerializeField] public GameObject Off { get; set; }

        private bool _isOn;
        private float _timer;

        private new void Start()
        {
            base.Start();
            TurnOff();
            _timer = HauntManager.Instance.HauntTimer;
        }

        public override void Interact()
        {
            if (_isOn) TurnOff();
            else TurnOn();
        }

        public void TurnOn()
        {
            _isOn = true;
            HauntManager.Instance.HauntActive = true;
            StartCoroutine(TVOnSound());
        }
        
        public void TurnOff()
        {
            On.SetActive(false);
            Off.SetActive(true);
            _isOn = false;
            HauntManager.Instance.HauntActive = false;

            AudioManager.PlayOneShot(FMODEvents.Instance.TVClick, transform.position);
            AudioManager.Instance.StopAmbience("TV Static");
        }

        private IEnumerator TVOnSound()
        {
            AudioManager.PlayOneShot(FMODEvents.Instance.TVClick, transform.position);
            
            yield return new WaitForSeconds(.15f);
            
            On.SetActive(true);
            Off.SetActive(false);
            AudioManager.Instance.InitialiseAmbience("TV Static", FMODEvents.Instance.TVStatic, transform);
        }
        private void FixedUpdate()
        {
            if (_isOn) _timer -= Time.deltaTime;
            if (_isOn && _timer <= 0 && HauntManager.Instance.DaughterInstance == null)
            {
                _timer = HauntManager.Instance.HauntTimer;
                TurnOff();
                HauntManager.Instance.ForceSpawnDaughter(default);
            }
        }
    }
}
