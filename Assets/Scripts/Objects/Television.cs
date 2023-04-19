using System.Collections;
using Audio;
using UnityEngine;

namespace Objects
{
    public class Television : Interactable
    {
        [field: SerializeField] public GameObject On { get; set; }
        [field: SerializeField] public GameObject Off { get; set; }

        private bool _isOn;

        private new void Start()
        {
            base.Start();
            TurnOff();
        }
        
        public override void Interact()
        {
            if (_isOn) TurnOff();
            else TurnOn();
        }

        public void TurnOn()
        {
            _isOn = true;
            StartCoroutine(TVOnSound());
        }
        
        public void TurnOff()
        {
            On.SetActive(false);
            Off.SetActive(true);
            _isOn = false;

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
    }
}
