using System;
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
        private InputAction _iDebugHauntTVOn;
        
        private InputAction _iDebugSpawnFather;
        private InputAction _iDebugSpawnMother;
        private InputAction _iDebugSpawnDaughter;
        
        private InputAction _iDebugCheatSleep;
        
        #endregion

        #region - UNITY Awake -
        
        private void Awake()
        {
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
            _iDebugHauntTVOn = _controls.Debug.HauntTVOn;
            _iDebugSpawnFather = _controls.Debug.SpawnFather;
            _iDebugSpawnMother = _controls.Debug.SpawnMother;
            _iDebugSpawnDaughter = _controls.Debug.SpawnDaughter;
            _iDebugCheatSleep = _controls.Debug.CheatSleep;
        }
        
        #endregion

        #region - UNITY Start -
        
        private void Start()
        {
            var rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
            if (string.IsNullOrEmpty(rebinds)) return;
            _controls.LoadBindingOverridesFromJson(rebinds);
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
            _iDebugHauntTVOn.Enable();
            _iDebugSpawnFather.Enable();
            _iDebugSpawnMother.Enable();
            _iDebugSpawnDaughter.Enable();
            _iDebugCheatSleep.Enable();
        }

        private void OnDisable()
        {
            if (Instance != this) return;
            
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
            _iDebugHauntTVOn.Disable();
            _iDebugSpawnFather.Disable();
            _iDebugSpawnMother.Disable();
            _iDebugSpawnDaughter.Disable();
            _iDebugCheatSleep.Disable();
        }
        
        #endregion

        #region - UNITY Update -
        
        private void Update()
        {
            MovementVector = _iPlayerMove.ReadValue<Vector3>();
        }
        
        #endregion
        
        #region - In-Game Initialising -

        public void InitialiseInMenu()
        {
            _iUIPause.performed += TitleManager.Instance.ToggleSettings;
        }

        public void InitialiseInGame()
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
            
            _iUIPause.performed -= TitleManager.Instance.ToggleSettings;
            _iUIPause.performed += UIManager.Instance.PauseGame;
            
            _iUIDebugInfo.performed += DebugManager.Instance.ToggleDebugInfo;
            
            _iDebugHauntLightsOff.started += HauntManager.Instance.ForceLightsOffHaunt;
            _iDebugHauntClockChime.started += HauntManager.Instance.ForceClockChimeHaunt;
            _iDebugHauntTVOn.started += HauntManager.Instance.ForceTVOnHaunt;
            _iDebugSpawnFather.started += HauntManager.Instance.ForceSpawnFather;
            _iDebugSpawnMother.started += HauntManager.Instance.ForceSpawnMother;
            _iDebugSpawnDaughter.started += HauntManager.Instance.ForceSpawnDaughter;
            _iDebugCheatSleep.started += DebugManager.CheatSleep;
        }

        public void Unregister()
        {
            _iPlayerCrouch.performed -= GameManager.Instance.Player.Crouch;
            _iPlayerCrouch.canceled -= GameManager.Instance.Player.PrepareStand;
            
            _iPlayerLeanLeft.started -= GameManager.Instance.Player.LeanLeft;
            _iPlayerLeanLeft.canceled -= GameManager.Instance.Player.StopLeaning;
            
            _iPlayerLeanRight.started -= GameManager.Instance.Player.LeanRight;
            _iPlayerLeanRight.canceled -= GameManager.Instance.Player.StopLeaning;
            
            _iPlayerBlink.started -= GameManager.Instance.Player.Blink;
            _iPlayerBlink.started -= UIManager.Instance.Blink;
            _iPlayerBlink.canceled -= GameManager.Instance.Player.StopBlinking;
            _iPlayerBlink.canceled -= UIManager.Instance.StopBlinking;
            
            _iPlayerWatch.started -= GameManager.Instance.Player.ToggleWatch;
            _iPlayerWatch.canceled -= GameManager.Instance.Player.ToggleWatch;
            
            _iPlayerInteract.started -= GameManager.Instance.Player.Interact;
            
            _iUIPause.performed -= UIManager.Instance.PauseGame;
            _iUIDebugInfo.performed -= DebugManager.Instance.ToggleDebugInfo;
            
            _iDebugHauntLightsOff.started -= HauntManager.Instance.ForceLightsOffHaunt;
            _iDebugHauntClockChime.started -= HauntManager.Instance.ForceClockChimeHaunt;
            _iDebugHauntTVOn.started -= HauntManager.Instance.ForceTVOnHaunt;
            _iDebugSpawnFather.started -= HauntManager.Instance.ForceSpawnFather;
            _iDebugSpawnMother.started -= HauntManager.Instance.ForceSpawnMother;
            _iDebugSpawnDaughter.started -= HauntManager.Instance.ForceSpawnDaughter;
            _iDebugCheatSleep.started -= DebugManager.CheatSleep;
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
        
        #region - Rebinding -

        public void TogglePlayerInputMap(bool toggle)
        {
            if (toggle) _controls.Player.Enable();
            else _controls.Player.Disable();
        }
        
        public InputAction GetInputReference(InputType input)
        {
            var inputReference = input switch
            {
                InputType.MoveForward => _iPlayerMove,
                InputType.MoveBackward => _iPlayerMove,
                InputType.MoveRight => _iPlayerMove,
                InputType.MoveLeft => _iPlayerMove,
                InputType.LeanRight => _iPlayerLeanRight,
                InputType.LeanLeft => _iPlayerLeanLeft,
                InputType.Crouch => _iPlayerCrouch,
                InputType.Interact => _iPlayerInteract,
                InputType.CloseEyes => _iPlayerBlink,
                InputType.Watch => _iPlayerWatch,
                _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
            };

            return inputReference;
        }
        
        #endregion
    }
}
