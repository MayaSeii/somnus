using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Objects
{
    public abstract class Interactable : MonoBehaviour
    {
        #region - VAR Components -
        
        [field: SerializeField] public Renderer MainRenderer { get; set; }
        
        private Camera _camera;
        private PlayerController _playerCon;
        private Transform _interactionPoint;
        private Collider _collider;
        
        #endregion
        
        #region - VAR UI Elements -
        
        protected Image InteractionCircle;
        private Image _interactionArrow;
        
        private float _targetCircleAlpha;
        private float _currentCircleAlpha;
        
        private float _targetArrowAlpha;
        private float _currentArrowAlpha;
        
        #endregion

        #region - UNITY Awake -
        
        private void Awake()
        {
            _camera = Camera.main;
            _playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _interactionPoint = transform.GetChild(0).CompareTag("Interaction Point") ? transform.GetChild(0) : transform;
            _collider = GetComponent<Collider>();

            CreateInteractionCircle();
            CreateInteractionArrow();
        }
        
        #endregion
        
        #region - UNITY Start -

        protected void Start()
        {
            InitialiseInteractionCircle();
            InitialiseInteractionArrow();
        }
        
        #endregion
        
        #region - UNITY Updates -

        private void Update()
        {
            UpdateInteractionCircleColour();
            UpdateInteractionArrowColour();
        }

        private void LateUpdate()
        {
            UpdateInteractionCirclePosition();
            _targetArrowAlpha = 0f;

            var showArrow = false;
            
            var vpPos = _camera.WorldToViewportPoint(_interactionPoint.position);
            
            if (vpPos.x is < 0f or > 1f || vpPos.y is < 0f or > 1f || vpPos.z <= 0f)
            {
                _targetCircleAlpha = 0f;
                showArrow = DistanceToPlayer() < 10;
            }
            else
            {
                _interactionArrow.enabled = false;
            }

            if (!showArrow && DistanceToPlayer() > 10)
            {
                _targetCircleAlpha = 0f;
                return;
            }

            var raycastHits = new List<RaycastHit>();
            
            GetBoundCorners().ForEach(c =>
            {
                var origin = _playerCon.GetCameraAbsolutePosition();
                var direction = c - origin;
                var layerMask = LayerMask.GetMask("Walls", "Interactable");
                var interactableLayer = LayerMask.NameToLayer("Interactable");
                
                if (!Physics.Raycast(origin, direction, out var raycastHit, 10, layerMask)) return;
                if (raycastHit.collider.gameObject.layer == interactableLayer) raycastHits.Add(raycastHit);
            });

            var enable = raycastHits.Any(r => r.transform == transform);
            
            if (!showArrow)
            {
                _targetCircleAlpha = enable ? (DistanceToPlayer() - 10f) / -5f : 0f;
            }
            else if (enable)
            {
                _targetArrowAlpha = (DistanceToPlayer() - 10f) / -5f;
                UpdateInteractionArrowPosition();
            }
            else
            {
                _targetArrowAlpha = 0f;
            }
        }
        
        #endregion

        #region - Abstractions -
        
        public abstract void Interact();
        
        #endregion
        
        #region - Creating Interaction UI -

        private void CreateInteractionCircle()
        {
            InteractionCircle = new GameObject { name = "Interactable Icon" }.AddComponent<Image>();
            InteractionCircle.transform.SetParent(GameObject.FindWithTag("UI Canvas").transform);
            InteractionCircle.sprite = Resources.Load<Sprite>("Sprites/InteractionCircleEmpty");
            InteractionCircle.gameObject.layer = LayerMask.NameToLayer("UI");
            InteractionCircle.enabled = false;
        }

        private void CreateInteractionArrow()
        {
            _interactionArrow = new GameObject { name = "Interactable Arrow" }.AddComponent<Image>();
            _interactionArrow.transform.SetParent(GameObject.FindWithTag("UI Canvas").transform);
            _interactionArrow.sprite = Resources.Load<Sprite>("Sprites/InteractionArrow");
            _interactionArrow.gameObject.layer = LayerMask.NameToLayer("UI");
            _interactionArrow.enabled = false;
        }
        
        #endregion
        
        #region - Initialising Interaction UI -

        private void InitialiseInteractionCircle()
        {
            InteractionCircle.transform.localScale = Vector3.one;
            InteractionCircle.transform.position = _camera.WorldToScreenPoint(_interactionPoint.position);
            InteractionCircle.SetNativeSize();
        }
        
        private void InitialiseInteractionArrow()
        {
            _interactionArrow.transform.localScale = Vector3.one;
            _interactionArrow.SetNativeSize();
        }
        
        #endregion
        
        #region - Updating Interaction UI -

        private void UpdateInteractionCircleColour()
        {
            _currentCircleAlpha = Mathf.MoveTowards(_currentCircleAlpha, _targetCircleAlpha, 5f * Time.deltaTime);
            
            var colour = InteractionCircle.color;
            colour.a = _currentCircleAlpha;
            InteractionCircle.color = colour;

            if (InteractionCircle.enabled && InteractionCircle.color.a <= 0f) InteractionCircle.enabled = false;
            if (!InteractionCircle.enabled && InteractionCircle.color.a > 0f) InteractionCircle.enabled = true;
        }

        private void UpdateInteractionCirclePosition()
        {
            ToggleInteractionCircleFill(_playerCon.InteractableInRange == _collider);
            
            _camera.ResetWorldToCameraMatrix();
            InteractionCircle.transform.SetPositionAndRotation((Vector2) _camera.WorldToScreenPoint(_interactionPoint.position), Quaternion.identity);
        }

        private void ToggleInteractionCircleFill(bool active)
        {
            InteractionCircle.sprite =
                Resources.Load<Sprite>(active ? "Sprites/InteractionCircle" : "Sprites/InteractionCircleEmpty");
        }
        
        private void UpdateInteractionArrowPosition()
        {
            var screenPosition = _camera.WorldToScreenPoint(transform.position);
            var screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
            var screenBounds = screenCentre * 0.95f;
            
            screenPosition -= screenCentre;
            if (screenPosition.z < 0) screenPosition *= -1;
           
            var angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            var slope = Mathf.Tan(angle);

            var sign = screenPosition.x > 0 ? 1 : -1;
            screenPosition = new Vector3(screenBounds.x * sign, screenBounds.x * slope * sign, 0);
            
            if (screenPosition.y > screenBounds.y)
                screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            
            else if (screenPosition.y < -screenBounds.y)
                screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
            
            screenPosition += screenCentre;
            
            _interactionArrow.transform.position = screenPosition;
            _interactionArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        }
        
        private void UpdateInteractionArrowColour()
        {
            _currentArrowAlpha = Mathf.MoveTowards(_currentArrowAlpha, _targetArrowAlpha, 5f * Time.deltaTime);
            
            var colour = _interactionArrow.color;
            colour.a = _currentArrowAlpha;
            _interactionArrow.color = colour;

            if (_interactionArrow.enabled && _interactionArrow.color.a <= 0f) _interactionArrow.enabled = false;
            if (!_interactionArrow.enabled && _interactionArrow.color.a > 0f) _interactionArrow.enabled = true;
        }
        
        #endregion
        
        #region - Helper Methods -
        
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
            boundCorners.Add(_interactionPoint.position);

            return boundCorners;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_playerCon.transform.position, _interactionPoint.transform.position);
        }
        
        #endregion
    }
}