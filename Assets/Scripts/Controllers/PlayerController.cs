using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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

        private EventInstance _footstepsInstance;
    
        #endregion
        
        #region - VAR Looking -
    
        [field: SerializeField, Header("Looking")] public float CameraPitchLimit { get; set; }
    
        private float _cameraPitch;
    
        #endregion
    
        #region - VAR Blinking & Rest -

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
        private float _headBobCounter;
    
        #endregion
        
        #region - VAR Interacting -

        [field: SerializeField, Header("Interacting")] public LayerMask InteractionLayerMask { get; set; }
        [field: SerializeField] public float InteractionDistance { get; set; }
        
        private Collider _interactableInRange;
        
        #endregion
        
        #region - VAR Watch -
        
        [field: SerializeField, Header("Wrist Watch")] public Transform Arm { get; set; }
        [field: SerializeField] public Vector3 RestingArmPosition { get; set; }
        [field: SerializeField] public Vector3 RestingArmRotation { get; set; }
        [field: SerializeField] public Vector3 ActiveArmPosition { get; set; }
        [field: SerializeField] public Vector3 ActiveArmRotation { get; set; }

        private Vector3 _targetArmRotation;
        private Vector3 _targetArmPosition;
        private bool _isLookingAtArm;
        
        #endregion

        #region - UNITY Start -
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        
            _speed = Speed;
            _cameraHeight = CameraHeight;
            
            _targetArmRotation = RestingArmRotation;
            _targetArmPosition = RestingArmPosition;

            _footstepsInstance = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Footsteps);
            RuntimeManager.AttachInstanceToGameObject(_footstepsInstance, transform, _rb);
        }
    
        #endregion
    
        #region - UNITY Update -

        private void Update()
        {
            UpdateRest();
            UpdateInteractionArea();
            UpdateSounds();
        }

        private void LateUpdate()
        {
            UpdateCamera();
        }

        private void FixedUpdate()
        {
            UpdateLean();
            UpdateMovement();
            UpdateCrouching();
            CameraBobbing();
            UpdateWatchArm();
        }
    
        #endregion
        
        #region - UNITY Triggers -

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Room Area")) EnterRoomArea(other);
        }

        private static void EnterRoomArea(Component other)
        {
            var roomController = other.transform.GetComponentInParent<RoomController>();
            DebugManager.Instance.UpdateCurrentRoom(roomController.RoomName);
        }

        #endregion
    
        #region - Movement & Looking -
    
        private void UpdateMovement()
        {
            var velocity = transform.TransformDirection(ControlsManager.Instance.MovementVector * (_speed * Time.fixedDeltaTime));
            _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
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
            _collider.center = new Vector3(0, (CrouchColliderHeight - ColliderHeight) / 2f, 0);

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

        private bool HasLowCeilingAbove()
        {
            var bounds = _collider.bounds;
            var size = new Vector3(bounds.size.x / 2f - .2f, bounds.size.y / 2f, bounds.size.z / 2f  - .2f);
            return Physics.BoxCast(bounds.center, size, Vector3.up, Quaternion.identity, 2f);
        }

        private void UpdateCrouching()
        {
            if (!_willStand || HasLowCeilingAbove()) return;
        
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
            _headBobCounter += Time.fixedDeltaTime;

            if (_rb.velocity is { x: 0, z: 0 } || !Settings.EnableHeadBobbing)
            {
                _sin = Mathf.Lerp(_sin, _cameraHeight, Time.fixedDeltaTime * HeadBobSpeed);
                _headBobCounter = 0f;
            }
            else _sin = _cameraHeight + HeadBobHeight * Mathf.Sin(HeadBobFrequency * Time.fixedDeltaTime * _headBobCounter);
        
            var position = CameraTransform.localPosition;
            CameraTransform.localPosition = new Vector3(position.x, Mathf.Lerp(position.y, _sin, HeadBobSpeed * Time.fixedDeltaTime), position.z);
        }
    
        #endregion
        
        #region - Watch -
        
        public void ToggleWatch(InputAction.CallbackContext context)
        {
            _isLookingAtArm = !_isLookingAtArm;
            
            _targetArmRotation = _isLookingAtArm ? ActiveArmRotation : RestingArmRotation;
            _targetArmPosition = _isLookingAtArm ? ActiveArmPosition : RestingArmPosition;
            
            UIManager.Instance.ToggleDoF(_isLookingAtArm);
        }

        private void UpdateWatchArm()
        {
            var pos = new Vector3(_targetArmPosition.x, _targetArmPosition.y, _targetArmPosition.z);
            Arm.localPosition = Vector3.Lerp(Arm.localPosition, pos, (_isLookingAtArm ? 12f : 4f) * Time.fixedDeltaTime);
            Arm.localRotation = Quaternion.Slerp(Arm.localRotation, Quaternion.Euler(_targetArmRotation), 7f * Time.fixedDeltaTime);
        }
        
        #endregion
        
        #region - Interacting -

        private void UpdateInteractionArea()
        {
            Physics.Raycast(CameraTransform.position, CameraTransform.forward, out var ray, InteractionDistance, InteractionLayerMask);
            _interactableInRange = ray.distance > 0 ? ray.collider : null;
            DebugManager.Instance.UpdateCurrentInteractable(ray.distance > 0 ? ray.collider.name : "None");
            UIManager.Instance.ToggleInteractionCircle(ray.distance > 0);
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (!_interactableInRange) return;
            _interactableInRange.GetComponent<Interactable>().Interact();
        }
        
        #endregion
        
        #region - Sounds -

        private void UpdateSounds()
        {
            if (ControlsManager.Instance.MovementVector != Vector3.zero)
            {
                _footstepsInstance.getPlaybackState(out var playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) _footstepsInstance.start();
            }
            else
            {
                _footstepsInstance.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }
        
        #endregion
    }
}
