using UnityEngine;

namespace Objects
{
    public class Door : Interactable
    {
        private Animator _animator;
        private Transform _player;
        private bool _isOpen;
        private string _closeDirection;

        private new void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void Interact()
        {
            if (!InteractableIcon.enabled) return;
            
            _isOpen = !_isOpen;

            var direction = transform.InverseTransformPoint(_player.position).x > 0 ? "Front" : "Back";
            _animator.Play(_isOpen ? $"Door Open {direction}" : $"Door Close {_closeDirection}", 0, 0);
            _closeDirection = direction;
        
            AudioManager.Instance.PlayOneShot(_isOpen ? FMODEvents.Instance.DoorOpen : FMODEvents.Instance.DoorClose, transform.position);
        }
    }
}
