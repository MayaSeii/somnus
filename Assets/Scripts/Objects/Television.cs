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
            On.SetActive(true);
            Off.SetActive(false);
            _isOn = true;
        }
        
        public void TurnOff()
        {
            On.SetActive(false);
            Off.SetActive(true);
            _isOn = false;
        }
    }
}
