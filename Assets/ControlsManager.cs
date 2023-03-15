using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    
    [field: SerializeField] public PlayerController Player { get; set; }
    
    public SettingsManager SettingsManager { get; set; }
    public Vector3 MovementVector { get; private set; }
    
    private Controls _controls;

    private InputAction _iPlayerMove;
    private InputAction _iPlayerCrouch;

    private InputAction _iUIPause;

    private void Awake()
    {
        Instance = this;
        SettingsManager = GetComponent<SettingsManager>();
        
        _controls = new Controls();

        _iPlayerMove = _controls.Player.Move;
        
        _iPlayerCrouch = _controls.Player.Crouch;
        _iPlayerCrouch.performed += Player.Crouch;
        _iPlayerCrouch.canceled += Player.MarkStand;
        
        _iUIPause = _controls.UI.Pause;
        _iUIPause.performed += SettingsManager.PauseGame;
    }

    private void OnEnable()
    {
        _iPlayerMove.Enable();
        _iPlayerCrouch.Enable();
        
        _iUIPause.Enable();
    }

    private void OnDisable()
    {
        _iPlayerMove.Disable();
        _iPlayerCrouch.Disable();
        
        _iUIPause.Disable();
    }

    private void Update()
    {
        MovementVector = _iPlayerMove.ReadValue<Vector3>();
    }

    public void ToggleCrouchToggle()
    {
        if (Settings.ToggleCrouch)
        {
            _iPlayerCrouch.performed -= Player.Crouch;
            _iPlayerCrouch.canceled -= Player.MarkStand;

            _iPlayerCrouch.started += Player.ToggleCrouch;
        }
        else
        {
            _iPlayerCrouch.performed += Player.Crouch;
            _iPlayerCrouch.canceled += Player.MarkStand;

            _iPlayerCrouch.started -= Player.ToggleCrouch;
        }
    }
}
