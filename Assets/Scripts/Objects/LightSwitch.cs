using System.Collections.Generic;
using Audio;
using Controllers;
using UnityEngine;

namespace Objects
{
    public class LightSwitch : Interactable
    {
        private List<Light> _associatedLights;
        private bool _state;
    
        private new void Start()
        {
            base.Start();
            _associatedLights = transform.parent.GetComponent<RoomController>().CeilingLamps;
            _state = true;
        }

        public override void Interact()
        {
            if (!InteractionCircle.enabled) return;
            
            _state = !_state;
            _associatedLights.ForEach(l => l.gameObject.SetActive(_state));
        
            Transform trans;
            (trans = transform).GetChild(1).rotation = Quaternion.Euler(_state ? -90 : 90, 0, 90);
            AudioManager.PlayOneShot(_state ? FMODEvents.Instance.LightSwitchOn : FMODEvents.Instance.LightSwitchOff, trans.position);
        }

        public void ForceOff()
        {
            _state = false;
            _associatedLights.ForEach(l => l.gameObject.SetActive(_state));
            
            Transform trans;
            (trans = transform).GetChild(1).rotation = Quaternion.Euler(90, 0, 90);
            AudioManager.PlayOneShot(FMODEvents.Instance.LightSwitchOff, trans.position);
        }
        public bool GetState()
        {
            return _state;
        }
    }
}
