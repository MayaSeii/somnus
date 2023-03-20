using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance;

        private UIManager _interfaceManager;
        private Controls _controls;
        
        #region - VAR Player Inputs -
        
        [field: SerializeField] public PlayerController Player { get; set; }
        public Vector3 MovementVector { get; private set; }

        private InputAction _iPlayerMove;
        private InputAction _iPlayerCrouch;
        private InputAction _iPlayerLeanLeft;
        private InputAction _iPlayerLeanRight;
        private InputAction _iPlayerBlink;
        private InputAction _iPlayerWatch;
        
        #endregion

        #region - VAR UI Inputs -
        
        private InputAction _iUIPause;
        
        #endregion

        #region - UNITY Awake -
        
        private void Awake()
        {
            Instance = this;
            _interfaceManager = GetComponent<UIManager>();
        
            _controls = new Controls();

            _iPlayerMove = _controls.Player.Move;
        
            _iPlayerCrouch = _controls.Player.Crouch;
            _iPlayerCrouch.performed += Player.Crouch;
            _iPlayerCrouch.canceled += Player.PrepareStand;

            _iPlayerLeanLeft = _controls.Player.LeanLeft;
            _iPlayerLeanLeft.started += Player.LeanLeft;
            _iPlayerLeanLeft.canceled += Player.StopLeaning;
        
            _iPlayerLeanRight = _controls.Player.LeanRight;
            _iPlayerLeanRight.started += Player.LeanRight;
            _iPlayerLeanRight.canceled += Player.StopLeaning;

            _iPlayerBlink = _controls.Player.Blink;
            _iPlayerBlink.started += Player.Blink;
            _iPlayerBlink.started += _interfaceManager.Blink;
            _iPlayerBlink.canceled += Player.StopBlinking;
            _iPlayerBlink.canceled += _interfaceManager.StopBlinking;

            _iPlayerWatch = _controls.Player.Watch;
            _iPlayerWatch.started += Player.ToggleWatch;
            _iPlayerWatch.canceled += Player.ToggleWatch;
            
            _iUIPause = _controls.UI.Pause;
            _iUIPause.performed += _interfaceManager.PauseGame;
            
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        #endregion
        
        #region - UNITY OnEnable & OnDisable -

        private void OnEnable()
        {
            _iPlayerMove.Enable();
            _iPlayerCrouch.Enable();
            _iPlayerLeanLeft.Enable();
            _iPlayerLeanRight.Enable();
            _iPlayerBlink.Enable();
            _iPlayerWatch.Enable();
        
            _iUIPause.Enable();
        }

        private void OnDisable()
        {
            _iPlayerMove.Disable();
            _iPlayerCrouch.Disable();
            _iPlayerLeanLeft.Disable();
            _iPlayerLeanRight.Disable();
            _iPlayerBlink.Disable();
            _iPlayerWatch.Disable();
        
            _iUIPause.Disable();
        }
        
        #endregion

        #region - UNITY Update -
        
        private void Update()
        {
            MovementVector = _iPlayerMove.ReadValue<Vector3>();
        }
        
        #endregion

        #region - Crouch Mode -
        
        public void SetCrouchMode()
        {
            if (Settings.ToggleCrouch)
            {
                _iPlayerCrouch.performed -= Player.Crouch;
                _iPlayerCrouch.canceled -= Player.PrepareStand;

                _iPlayerCrouch.started += Player.ToggleCrouch;
            }
            else
            {
                _iPlayerCrouch.performed += Player.Crouch;
                _iPlayerCrouch.canceled += Player.PrepareStand;

                _iPlayerCrouch.started -= Player.ToggleCrouch;
            }
        }
        
        #endregion
    }
}
