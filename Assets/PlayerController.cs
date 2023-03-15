using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Transform CameraTransform { get; set; }
    [field: SerializeField] public PostProcessVolume PostProcessVolume { get; set; }
    private CapsuleCollider _collider { get; set; }
    
    private Rigidbody _rb;
    private float _sin;

    private float _cameraPitch;

    private float _counter;
    private float _cameraPos;
    private float _speed;
    private float _targetVignette;

    private Vignette _vignette;
    private bool _willStand;
    
    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        
        _targetVignette = 0.27f;
        _speed = 200f;
        _cameraPos = .75f;
        _sin = 0f;
        _counter = 0f;
        
        PostProcessVolume.profile.TryGetSettings(out _vignette);
        
        Cursor.visible = false;
        _rb = GetComponent<Rigidbody>();
    }

    public void ToggleCrouch(InputAction.CallbackContext context)
    {
        if (Math.Abs(_collider.height - 1) < .1f) MarkStand(context);
        else Crouch(context);
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        _cameraPos = .15f;
        _speed = 100f;

        _targetVignette = Settings.EnableVignette ? 0.5f : 0.27f;

        _collider.height = 1;
        _collider.center = new Vector3(0, -0.5f, 0);
    }

    public void MarkStand(InputAction.CallbackContext context)
    {
        _willStand = true;
    }
    
    public void Stand()
    {
        _cameraPos = .75f;
        _speed = 200f;

        _targetVignette = 0.27f;
        
        _collider.height = 2;
        _collider.center = Vector3.zero;
    }

    private void Update()
    {
        UpdateCamera();
        UpdatePostProcessing();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateCrouching();
        CameraBobbing();
    }

    private void UpdateCrouching()
    {
        var bounds = _collider.bounds;
        var size = new Vector3(bounds.size.x / 2f - .2f, bounds.size.y, bounds.size.z  - .2f);
        var raycastHit = Physics.BoxCast(bounds.center, size, Vector3.up, Quaternion.identity, 2f, -1);
        if (!_willStand || raycastHit) return;
        
        Stand();
        _willStand = false;
    }

    private void CameraBobbing()
    {
        _counter += Time.fixedDeltaTime;

        if (_rb.velocity == Vector3.zero || !Settings.EnableHeadBobbing)
        {
            _sin = Mathf.Lerp(_sin, _cameraPos, Time.fixedDeltaTime * 20f);
            _counter = 0f;
        }
        else _sin = _cameraPos + .05f * Mathf.Sin(500f * Time.fixedDeltaTime * _counter);
        
        var position = CameraTransform.position;
        CameraTransform.position = new Vector3(position.x, Mathf.Lerp(position.y, _sin, 20f * Time.fixedDeltaTime), position.z);
    }

    private void UpdateMovement()
    {
        _rb.velocity = transform.TransformDirection(ControlsManager.Instance.MovementVector * (_speed * Time.fixedDeltaTime));
    }

    private void UpdateCamera()
    {
        var targetMouseDelta = Mouse.current.delta.ReadValue() * Time.deltaTime;
        
        _cameraPitch -= targetMouseDelta.y * (Settings.InvertY ? -1 : 1) * Settings.MouseSensitivityY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -70.0f, 70.0f);
        
        CameraTransform.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * (targetMouseDelta.x * Settings.MouseSensitivityX));
    }

    private void UpdatePostProcessing()
    {
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _targetVignette, 15f * Time.deltaTime);
    }
}
