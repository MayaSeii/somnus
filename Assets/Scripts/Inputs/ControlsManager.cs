using General;
using Haunts;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance;

        private Controls _controls;
        
        #region - VAR Player Inputs -
        
        public Vector3 MovementVector { get; private set; }

        private InputAction _iPlayerMove;
        private InputAction _iPlayerCrouch;
        private InputAction _iPlayerLeanLeft;
        private InputAction _iPlayerLeanRight;
        private InputAction _iPlayerBlink;
        private InputAction _iPlayerWatch;
        private InputAction _iPlayerInteract;
        
        #endregion

        #region - VAR UI Inputs -
        
        private InputAction _iUIPause;
        private InputAction _iUIDebugInfo;
        
        #endregion
        
        #region - VAR Debug Inputs -

        private InputAction _iDebugHauntLightsOff;
        private InputAction _iDebugHauntClockChime;
        
        #endregion

        #region - UNITY Awake -
        
        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Controls Manager in the scene.");
            Instance = this;
        
            _controls = new Controls();

            _iPlayerMove = _controls.Player.Move;
            _iPlayerCrouch = _controls.Player.Crouch;
            _iPlayerLeanLeft = _controls.Player.LeanLeft;
            _iPlayerLeanRight = _controls.Player.LeanRight;
            _iPlayerBlink = _controls.Player.Blink;
            _iPlayerWatch = _controls.Player.Watch;
            _iPlayerInteract = _controls.Player.Interact;
            
            _iUIPause = _controls.UI.Pause;
            _iUIDebugInfo = _controls.UI.DebugInfo;

            _iDebugHauntLightsOff = _controls.Debug.HauntLightsOff;
            _iDebugHauntClockChime = _controls.Debug.HauntClockChime;
            
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        #endregion

        #region - UNITY Start -
        
        private void Start()
        {
            _iPlayerCrouch.performed += GameManager.Instance.Player.Crouch;
            _iPlayerCrouch.canceled += GameManager.Instance.Player.PrepareStand;
            
            _iPlayerLeanLeft.started += GameManager.Instance.Player.LeanLeft;
            _iPlayerLeanLeft.canceled += GameManager.Instance.Player.StopLeaning;
            
            _iPlayerLeanRight.started += GameManager.Instance.Player.LeanRight;
            _iPlayerLeanRight.canceled += GameManager.Instance.Player.StopLeaning;
            
            _iPlayerBlink.started += GameManager.Instance.Player.Blink;
            _iPlayerBlink.started += UIManager.Instance.Blink;
            _iPlayerBlink.canceled += GameManager.Instance.Player.StopBlinking;
            _iPlayerBlink.canceled += UIManager.Instance.StopBlinking;
            
            _iPlayerWatch.started += GameManager.Instance.Player.ToggleWatch;
            _iPlayerWatch.canceled += GameManager.Instance.Player.ToggleWatch;
            
            _iPlayerInteract.started += GameManager.Instance.Player.Interact;
            
            _iUIPause.performed += UIManager.Instance.PauseGame;
            _iUIDebugInfo.performed += DebugManager.Instance.ToggleDebugInfo;
            
            _iDebugHauntLightsOff.started += HauntManager.Instance.ForceLightsOffHaunt;
            _iDebugHauntClockChime.started += HauntManager.Instance.ForceClockChimeHaunt;
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
            _iPlayerInteract.Enable();
        
            _iUIPause.Enable();
            _iUIDebugInfo.Enable();
            
            _iDebugHauntLightsOff.Enable();
            _iDebugHauntClockChime.Enable();
        }

        private void OnDisable()
        {
            _iPlayerMove.Disable();
            _iPlayerCrouch.Disable();
            _iPlayerLeanLeft.Disable();
            _iPlayerLeanRight.Disable();
            _iPlayerBlink.Disable();
            _iPlayerWatch.Disable();
            _iPlayerInteract.Disable();
        
            _iUIPause.Disable();
            _iUIDebugInfo.Disable();
            
            _iDebugHauntLightsOff.Disable();
            _iDebugHauntClockChime.Disable();
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
            if (Config.ToggleCrouch)
            {
                _iPlayerCrouch.performed -= GameManager.Instance.Player.Crouch;
                _iPlayerCrouch.canceled -= GameManager.Instance.Player.PrepareStand;

                _iPlayerCrouch.started += GameManager.Instance.Player.ToggleCrouch;
            }
            else
            {
                _iPlayerCrouch.performed += GameManager.Instance.Player.Crouch;
                _iPlayerCrouch.canceled += GameManager.Instance.Player.PrepareStand;

                _iPlayerCrouch.started -= GameManager.Instance.Player.ToggleCrouch;
            }
        }
        
        #endregion
    }
}
