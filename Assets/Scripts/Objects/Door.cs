using Audio;
using General;
using UnityEngine;

namespace Objects
{
    public class Door : Interactable
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

            var direction = transform.InverseTransformPoint(GameManager.Instance.Player.transform.position).x > 0 ? "Front" : "Back";
            _animator.Play(_isOpen ? $"Door Open {direction}" : $"Door Close {_closeDirection}", 0, 0);
            _closeDirection = direction;
        
            AudioManager.PlayOneShot(_isOpen ? FMODEvents.Instance.DoorOpen : FMODEvents.Instance.DoorClose, transform.position);
        }
    }
}
