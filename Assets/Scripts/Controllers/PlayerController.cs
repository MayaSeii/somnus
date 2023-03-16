using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraTransform { get; set; }
    
        #region - VAR Physics -
    
        private CapsuleCollider _collider;
        private Rigidbody _rb;
    
        private float _cameraHeight;
        private float _speed;
    
        #endregion
        
        #region - VAR Looking -
    
        [field: SerializeField, Header("Looking")] public float CameraPitchLimit { get; set; }
    
        private float _cameraPitch;
    
        #endregion
    
        #region - VAR Blinking & Rest -

        private float _blinkCounter;
        private float _restAchieved;
        private bool _isBlinking;
    
        #endregion
    
        #region - VAR Standing -
    
        [field: SerializeField, Header("Standing")] public float CameraHeight { get; set; }
        [field: SerializeField] public float ColliderHeight { get; set; }
        [field: SerializeField] public float Speed { get; set; }
    
        #endregion
    
        #region - VAR Crouching -
    
        [field: SerializeField, Header("Crouching")] public float CrouchCameraHeight { get; set; }
        [field: SerializeField] public float CrouchColliderHeight { get; set; }
        [field: SerializeField] public float CrouchSpeed { get; set; }
    
        private bool _willStand;
    
        #endregion
    
        #region - VAR Leaning -
    
        [field: SerializeField, Header("Leaning")] public Transform LeanPivot { get; set; } 
        [field: SerializeField] public float LeanSmoothing { get; set; }
        [field: SerializeField] public float LeanAngle { get; set; }
    
        private float _leanVelocity;
        private float _currentLean;
        private float _targetLeanAngle;
    
        #endregion
    
        #region - VAR Head Bobbing -
    
        [field: SerializeField, Header("Head Bobbing")] public float HeadBobHeight { get; set; }
        [field: SerializeField] public float HeadBobFrequency { get; set; }
        [field: SerializeField] public float HeadBobSpeed { get; set; }
    
        private float _sin;
    
        #endregion

        #region - UNITY Start -
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        
            _speed = Speed;
            _cameraHeight = CameraHeight;
        }
    
        #endregion
    
        #region - UNITY Update -

        private void Update()
        {
            UpdateRest();
            UpdateCamera();
        }

        private void FixedUpdate()
        {
            UpdateLean();
            UpdateMovement();
            UpdateCrouching();
            CameraBobbing();
        }
    
        #endregion
    
        #region - Movement & Looking -
    
        private void UpdateMovement()
        {
            _rb.velocity = transform.TransformDirection(ControlsManager.Instance.MovementVector * (_speed * Time.fixedDeltaTime));
        }

        private void UpdateCamera()
        {
            var targetMouseDelta = Mouse.current.delta.ReadValue() * Time.deltaTime;
        
            _cameraPitch -= targetMouseDelta.y * (Settings.InvertY ? -1 : 1) * Settings.MouseSensitivityY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -CameraPitchLimit, CameraPitchLimit);
        
            CameraTransform.localEulerAngles = Vector3.right * _cameraPitch;
            transform.Rotate(Vector3.up * (targetMouseDelta.x * (Settings.InvertX ? -1 : 1) * Settings.MouseSensitivityX));
        }
    
        #endregion
    
        #region - Blinking & Rest -
    
        public void Blink(InputAction.CallbackContext context)
        {
            _isBlinking = true;
        }
    
        public void StopBlinking(InputAction.CallbackContext context)
        {
            _isBlinking = false;
        }

        private void UpdateRest()
        {
            if (!_isBlinking) return;
        
            _restAchieved += Time.deltaTime;
            UIManager.Instance.UpdateRestMeter(_restAchieved);
        }
    
        #endregion
    
        #region - Crouching -
    
        public void ToggleCrouch(InputAction.CallbackContext context)
        {
            if (Math.Abs(_collider.height - 1) < .1f) PrepareStand(context);
            else Crouch(context);
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            _cameraHeight = CrouchCameraHeight;
            _speed = CrouchSpeed;

            _collider.height = CrouchColliderHeight;
            _collider.center = new Vector3(0, (CrouchCameraHeight - ColliderHeight) / 2f, 0);

            UIManager.Instance.ToggleCrouchVignette(true);
        }

        public void PrepareStand(InputAction.CallbackContext context)
        {
            _willStand = true;
        }
    
        private void Stand()
        {
            _cameraHeight = CameraHeight;
            _speed = Speed;
        
            _collider.height = ColliderHeight;
            _collider.center = Vector3.zero;
        
            UIManager.Instance.ToggleCrouchVignette(false);
        }

        private bool CanStand()
        {
            var bounds = _collider.bounds;
            var size = new Vector3(bounds.size.x / 2f - .2f, bounds.size.y, bounds.size.z  - .2f);
            return !Physics.BoxCast(bounds.center, size, Vector3.up, Quaternion.identity, 2f, -1);
        }

        private void UpdateCrouching()
        {
            if (!_willStand || !CanStand()) return;
        
            Stand();
            _willStand = false;
        }
    
        #endregion
    
        #region - Leaning -

        public void LeanLeft(InputAction.CallbackContext context)
        {
            _targetLeanAngle = LeanAngle;
        }
    
        public void LeanRight(InputAction.CallbackContext context)
        {
            _targetLeanAngle = -LeanAngle;
        }
    
        public void StopLeaning(InputAction.CallbackContext context)
        {
            _targetLeanAngle = 0f;
        }

        private void UpdateLean()
        {
            _currentLean = Mathf.SmoothDamp(_currentLean, _targetLeanAngle, ref _leanVelocity, LeanSmoothing);
            LeanPivot.localRotation = Quaternion.Euler(0f, 0f, _currentLean);
        }
    
        #endregion
    
        #region - Head Bobbing -
    
        private void CameraBobbing()
        {
            _blinkCounter += Time.fixedDeltaTime;

            if (_rb.velocity == Vector3.zero || !Settings.EnableHeadBobbing)
            {
                _sin = Mathf.Lerp(_sin, _cameraHeight, Time.fixedDeltaTime * HeadBobSpeed);
                _blinkCounter = 0f;
            }
            else _sin = _cameraHeight + HeadBobHeight * Mathf.Sin(HeadBobFrequency * Time.fixedDeltaTime * _blinkCounter);
        
            var position = CameraTransform.position;
            CameraTransform.position = new Vector3(position.x, Mathf.Lerp(position.y, _sin, HeadBobSpeed * Time.fixedDeltaTime), position.z);
        }
    
        #endregion
    }
}
