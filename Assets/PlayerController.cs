using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Transform CameraTransform { get; set; }
    [field: SerializeField] public PostProcessVolume PostProcessVolume { get; set; }
    
    private Controls _controls;

    private InputAction _iMove;
    private InputAction _iCrouch;
    
    private Rigidbody _rb;
    private float _sin;

    private const float MouseSensitivity = 5f;
    private float _cameraPitch;

    private float _counter;
    private float _cameraPos;
    private float _speed;
    private float _targetVignette;

    private Vignette _vignette;

    private void Awake()
    {
        _controls = new Controls();
        
        _iMove = _controls.Player.Move;
        
        _iCrouch = _controls.Player.Crouch;
        _iCrouch.performed += Crouch;
        _iCrouch.canceled += Stand;
    }

    private void Start()
    {
        _targetVignette = 0.27f;
        _speed = 200f;
        _cameraPos = .75f;
        _sin = 0f;
        _counter = 0f;
        
        PostProcessVolume.profile.TryGetSettings(out _vignette);
        
        Cursor.visible = false;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _iMove.Enable();
        _iCrouch.Enable();
    }

    private void OnDisable()
    {
        _iMove.Disable();
        _iCrouch.Disable();
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        _cameraPos = .15f;
        _speed = 100f;

        _targetVignette = 0.5f;
    }
    
    private void Stand(InputAction.CallbackContext context)
    {
        _cameraPos = .75f;
        _speed = 200f;

        _targetVignette = 0.27f;
    }

    private void Update()
    {

        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _targetVignette, 15f * Time.deltaTime);
        
        var targetMouseDelta = Mouse.current.delta.ReadValue() * Time.deltaTime;
        
        _cameraPitch -= targetMouseDelta.y * MouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -60.0f, 60.0f);
        
        CameraTransform.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * (targetMouseDelta.x * MouseSensitivity));
    }
    
    private void FixedUpdate()
    {
        _counter += Time.fixedDeltaTime;
        _rb.velocity = transform.TransformDirection(_iMove.ReadValue<Vector3>() * (_speed * Time.fixedDeltaTime));

        if (_rb.velocity == Vector3.zero)
        {
            _sin = Mathf.Lerp(_sin, _cameraPos, Time.fixedDeltaTime * 20f);
            _counter = 0f;
        }
        else _sin = _cameraPos + .05f * Mathf.Sin(500f * Time.fixedDeltaTime * _counter);
        
        var position = CameraTransform.position;
        CameraTransform.position = new Vector3(position.x, Mathf.Lerp(position.y, _sin, 20f * Time.fixedDeltaTime), position.z);
    }
}
