using UnityEngine;

namespace Objects
{
    public class Toilet : Interactable
    {
        [field: SerializeField] public Transform ToiletCover { get; private set; }

        private bool _isClosed;
        private float _targetAngle;

        private void FixedUpdate()
        {
            ToiletCover.rotation = Quaternion.Slerp(ToiletCover.rotation, Quaternion.Euler(_targetAngle, 0f, 0f), .15f);
        }

        public override void Interact()
        {
            _isClosed = !_isClosed;
            _targetAngle = _isClosed ? -90 : 0;
        }
    }
}

