using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Objects
{
    public abstract class Interactable : MonoBehaviour
    {
        [field: SerializeField] public Renderer MainRenderer { get; set; }
        
        protected Image InteractableIcon;
        
        private Camera _camera;
        private PlayerController _playerCon;
        private Collider _collider;
        private Transform _interactionPoint;
        
        private void Awake()
        {
            _camera = Camera.main;
            _playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _collider = GetComponent<Collider>();
            
            InteractableIcon = new GameObject { name = "Interactable Icon" }.AddComponent<Image>();
            InteractableIcon.transform.SetParent(GameObject.FindWithTag("UI Canvas").transform);
            InteractableIcon.sprite = Resources.Load<Sprite>("Sprites/InteractionCircleEmpty");
            InteractableIcon.enabled = false;
            InteractableIcon.gameObject.layer = LayerMask.NameToLayer("UI");

            _interactionPoint = transform.GetChild(0).CompareTag("Interaction Point") ? transform.GetChild(0) : transform;
        }

        protected void Start()
        {
            PrepareIcon();
        }

        public abstract void Interact();

        private void LateUpdate()
        {
            UpdateIcon();
            
            var vpPos = _camera.WorldToViewportPoint(_interactionPoint.position);
            
            if (vpPos.x is < 0f or > 1f || vpPos.y is < 0f or > 1f || vpPos.z <= 0f)
            {
                InteractableIcon.enabled = false;
                return;
            }

            if (DistanceToPlayer() > 10)
            {
                InteractableIcon.enabled = false;
                return;
            }

            var raycastHits = new List<RaycastHit>();
            
            GetBoundCorners().ForEach(c =>
            {
                if (Physics.Raycast(_camera.transform.position, c - _camera.transform.position, out var raycastHit, 1000, LayerMask.GetMask("Walls", "Interactable")))
                    raycastHits.Add(raycastHit);
            });

            var visible = false;
            
            raycastHits.ForEach(r =>
            {
                if (r.transform == transform)
                {
                    visible = true;
                }
            });

            InteractableIcon.enabled = visible;
        }

        private List<Vector3> GetBoundCorners()
        {
            var boundCorners = new List<Vector3>();
            
            var bounds = MainRenderer.bounds;
            
            var boundPoint1 = bounds.min;
            var boundPoint2 = bounds.max;
            var boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
            var boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
            var boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
            var boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
            var boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
            var boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);
            
            boundCorners.Add(boundPoint1);
            boundCorners.Add(boundPoint2);
            boundCorners.Add(boundPoint3);
            boundCorners.Add(boundPoint4);
            boundCorners.Add(boundPoint5);
            boundCorners.Add(boundPoint6);
            boundCorners.Add(boundPoint7);
            boundCorners.Add(boundPoint8);

            return boundCorners;
        }

        private void PrepareIcon()
        {
            InteractableIcon.transform.localScale = Vector3.one;
            InteractableIcon.transform.position = _camera.WorldToScreenPoint(_interactionPoint.position);
            InteractableIcon.SetNativeSize();
        }

        private void UpdateIcon()
        {
            ToggleIcon(_playerCon.InteractableInRange == _collider);

            InteractableIcon.color = new Color(1, 1, 1, (DistanceToPlayer() - 10f) / (5f - 10f) * (1f - 0f) + 0f);
            
            
            _camera.ResetWorldToCameraMatrix();
            InteractableIcon.transform.SetPositionAndRotation((Vector2) _camera.WorldToScreenPoint(_interactionPoint.position), Quaternion.identity);
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_playerCon.transform.position, _interactionPoint.transform.position);
        }

        private void ToggleIcon(bool active)
        {
            InteractableIcon.sprite =
                Resources.Load<Sprite>(active ? "Sprites/InteractionCircle" : "Sprites/InteractionCircleEmpty");
        }
    }
}