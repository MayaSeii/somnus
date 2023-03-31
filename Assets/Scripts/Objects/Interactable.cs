using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Objects
{
    public abstract class Interactable : MonoBehaviour
    {
        #region - VAR Components -
        
        [field: SerializeField] public Renderer MainRenderer { get; set; }
        
        public bool IsThrowable { get; protected set; }
        
        private Transform _interactionPoint;
        private Collider _collider;
        private Canvas _gameplayUICanvas;
        
        #endregion
        
        #region - VAR UI Elements -

        protected bool ShowArrow;
        
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
            _interactionPoint = transform.GetChild(0).CompareTag("Interaction Point") ? transform.GetChild(0) : transform;
            _collider = GetComponent<Collider>();
            _gameplayUICanvas = GameObject.FindWithTag("Gameplay UI").GetComponent<Canvas>();

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
            if (GameManager.Instance.Player.HeldObject)
            {
                _targetArrowAlpha = 0f;
                _targetCircleAlpha = 0f;
                return;
            }
            
            UpdateInteractionCirclePosition();
            _targetArrowAlpha = 0f;

            var showArrow = false;
            
            var vpPos = GameManager.Instance.MainCamera.WorldToViewportPoint(_interactionPoint.position);
            
            if (vpPos.x is < 0f or > 1f || vpPos.y is < 0f or > 1f || vpPos.z <= 0f)
            {
                _targetCircleAlpha = 0f;
                showArrow = DistanceToPlayer() < 7;
            }
            else
            {
                _interactionArrow.enabled = false;
            }

            if (!showArrow && DistanceToPlayer() > 7)
            {
                _targetCircleAlpha = 0f;
                return;
            }

            var raycastHits = new List<RaycastHit>();
            
            GetBoundCorners().ForEach(c =>
            {
                var origin = GameManager.Instance.Player.GetCameraAbsolutePosition();
                var direction = c - origin;
                var layerMask = LayerMask.GetMask("Walls", "Interactable");
                var interactableLayer = LayerMask.NameToLayer("Interactable");
                
                if (!Physics.Raycast(origin, direction, out var raycastHit, 7, layerMask)) return;
                if (raycastHit.collider.gameObject.layer == interactableLayer) raycastHits.Add(raycastHit);
            });

            var enable = raycastHits.Any(r => r.transform == transform);
            
            if (!showArrow)
            {
                _targetCircleAlpha = enable ? (DistanceToPlayer() - 7f) / -5f : 0f;
            }
            else if (enable)
            {
                _targetArrowAlpha = (DistanceToPlayer() - 5f) / -5f;
                UpdateInteractionArrowPosition();
            }
            else
            {
                _targetArrowAlpha = 0f;
            }

            if (!ShowArrow) _targetArrowAlpha = 0f;
        }
        
        #endregion

        #region - Abstractions -
        
        public abstract void Interact();
        
        #endregion
        
        #region - Creating Interaction UI -

        private void CreateInteractionCircle()
        {
            InteractionCircle = new GameObject { name = "Interactable Icon" }.AddComponent<Image>();
            InteractionCircle.transform.SetParent(GameObject.FindWithTag("Gameplay UI").transform);
            InteractionCircle.sprite = Resources.Load<Sprite>("Sprites/InteractionCircleEmpty");
            InteractionCircle.gameObject.layer = LayerMask.NameToLayer("UI");
            InteractionCircle.enabled = false;
        }

        private void CreateInteractionArrow()
        {
            _interactionArrow = new GameObject { name = "Interactable Arrow" }.AddComponent<Image>();
            _interactionArrow.transform.SetParent(GameObject.FindWithTag("Gameplay UI").transform);
            _interactionArrow.sprite = Resources.Load<Sprite>("Sprites/InteractionArrow");
            _interactionArrow.gameObject.layer = LayerMask.NameToLayer("UI");
            _interactionArrow.enabled = false;

            ShowArrow = true;
        }
        
        #endregion
        
        #region - Initialising Interaction UI -
    
        private void InitialiseInteractionCircle()
        {
            InteractionCircle.transform.localScale = Vector3.one;
            InteractionCircle.transform.position = GameManager.Instance.MainCamera.WorldToScreenPoint(_interactionPoint.position);
            InteractionCircle.SetNativeSize();
            InteractionCircle.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
        }
        
        private void InitialiseInteractionArrow()
        {
            _interactionArrow.transform.localScale = Vector3.one;
            _interactionArrow.SetNativeSize();
            _interactionArrow.GetComponent<RectTransform>().sizeDelta = new Vector2(16, 16);
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
            ToggleInteractionCircleFill(GameManager.Instance.Player.InteractableInRange == _collider);
            
            GameManager.Instance.MainCamera.ResetWorldToCameraMatrix();
            GameManager.Instance.GameplayUICamera.ResetWorldToCameraMatrix();
            InteractionCircle.transform.position = WorldToCamPoint(_interactionPoint.position);
        }

        private void ToggleInteractionCircleFill(bool active)
        {
            InteractionCircle.sprite =
                Resources.Load<Sprite>(active ? "Sprites/InteractionCircle" : "Sprites/InteractionCircleEmpty");
        }
        
        private void UpdateInteractionArrowPosition()
        {
            var screenPosition = GameManager.Instance.MainCamera.WorldToScreenPoint(transform.position);
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
            
            var screen = screenPosition;
            screen.z = (_gameplayUICanvas.transform.position - GameManager.Instance.MainCamera.transform.position).magnitude;
            
            _interactionArrow.transform.position = GameManager.Instance.GameplayUICamera.ScreenToWorldPoint(screen);
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
            return Vector3.Distance(GameManager.Instance.Player.transform.position, _interactionPoint.transform.position);
        }

        private Vector3 WorldToCamPoint(Vector3 pos)
        {
            var screen = GameManager.Instance.MainCamera.WorldToScreenPoint(pos);
            screen.z = (_gameplayUICanvas.transform.position - GameManager.Instance.MainCamera.transform.position).magnitude;
            return GameManager.Instance.GameplayUICamera.ScreenToWorldPoint(screen);
        }
        
        #endregion
    }
}