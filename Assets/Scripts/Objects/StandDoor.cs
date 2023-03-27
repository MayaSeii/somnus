using Audio;
using UnityEngine;

namespace Objects
{
    public class StandDoor : Interactable
    {
        private Animator _animator;
        private bool _isOpen;
        private string _closeDirection;

        private new void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
        }

        public override void Interact()
        {
            if (!InteractionCircle.enabled) return;
            
            _isOpen = !_isOpen;
            _animator.Play(_isOpen ? "Stand Door Open" : "Stand Door Close", 0, 0);
        
            AudioManager.PlayOneShot(_isOpen ? FMODEvents.Instance.StandDoorOpen : FMODEvents.Instance.StandDoorClose, transform.position);
        }
    }
}
