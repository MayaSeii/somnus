using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        #region - Interface Elements -
        
        [field: SerializeField, Header("Interface Elements")] public GameObject Crosshair { get; set; }
        
        #endregion
        
        #region - VAR Menus -
        
        [field: SerializeField, Header("Menus")] public GameObject PauseMenu { get; set; }
        [field: SerializeField] public GameObject SettingsMenu { get; set; }
    
        private bool _isPaused;
        
        #endregion
        
        #region - VAR Blinking & Rest -
    
        [field: SerializeField, Header("Blinking")] public Image BlinkOverlay { get; set; }
        [field: SerializeField] public Slider RestMeter { get; set; }
        [field: SerializeField] public Slider ArmRestMeter { get; set; }

        private float _targetBlinkOpacity;
    
        #endregion
    
        #region - VAR Post-Processing -
    
        [field: SerializeField, Header("Post-Processing")] public Transform ArmCam { get; set; }
        [field: SerializeField] public LayerMask DepthLayerMask { get; set; }
        
        private Transform _cam;
        
        private PostProcessVolume _postProcessVolume;
        private Vignette _vignette;
        private MotionBlur _motionBlur;
        private DepthOfField _depthOfField;
        private PSXEffects _psxEffects;
    
        private float _targetVignette;
        private float _targetDoF;
        private float _dynamicDoFDistance;
    
        #endregion

        #region - UNITY Awake -
    
        private void Awake()
        {
            Instance = this;
        }
    
        #endregion
    
        #region - UNITY Start -

        private void Start()
        {
            if (Camera.main != null) _cam = Camera.main.transform;
            
            _postProcessVolume = _cam != null ? _cam.GetComponent<PostProcessVolume>() : null;
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _vignette);
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _motionBlur);
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _depthOfField);
            _psxEffects = ArmCam.GetComponent<PSXEffects>();
            
            _targetVignette = 0.27f;
            _targetDoF = -1f;

            var cursorTexture = Resources.Load<Texture2D>("Sprites/Cursor");
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            Cursor.visible = false;
        }
    
        #endregion

        #region - UNITY Update -
    
        private void Update()
        {
            BlinkOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(BlinkOverlay.color.a, _targetBlinkOpacity, 20f * Time.deltaTime));
            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _targetVignette, 15f * Time.deltaTime);
            _depthOfField.focusDistance.value = Mathf.Lerp(_depthOfField.focusDistance.value, Math.Abs(_targetDoF + 1f) < .1f ? CalculateDoFDistance() : _targetDoF, 15 * Time.deltaTime);
        }
    
        #endregion
        
        #region - Pause Menu -
        
        public void PauseGame(InputAction.CallbackContext context)
        {
            if (!SettingsMenu.activeInHierarchy) TogglePauseMenu();
            else ToggleSettings();
        }

        public void TogglePauseMenu()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
            Cursor.visible = _isPaused;
            Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            
            PauseMenu.SetActive(_isPaused);
            Crosshair.SetActive(!_isPaused);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        #endregion
        
        #region - Settings Menu -
        
        public void ToggleSettings()
        {
            PauseMenu.SetActive(SettingsMenu.activeInHierarchy);
            SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
        }
        
        #endregion

        #region - Blinking & Rest -
    
        public void Blink(InputAction.CallbackContext context)
        {
            _targetBlinkOpacity = 1f;
            RestMeter.gameObject.SetActive(true);
        }

        public void StopBlinking(InputAction.CallbackContext context)
        {
            _targetBlinkOpacity = 0f;
            RestMeter.gameObject.SetActive(false);
        }

        public void UpdateRestMeter(float value)
        {
            RestMeter.value = Mathf.Clamp(value, RestMeter.minValue, RestMeter.maxValue);
            ArmRestMeter.value = Mathf.Clamp(value, RestMeter.minValue, RestMeter.maxValue);
        }
    
        #endregion

        #region - Post-Processing -
    
        public void ToggleCrouchVignette(bool enableVignette)
        {
            if (enableVignette) _targetVignette = Settings.EnableVignette ? 0.5f : 0.27f;
            else _targetVignette = 0.27f;
        }

        public void ToggleMotionBlur(bool enableMotionBlur)
        {
            _motionBlur.enabled.value = enableMotionBlur;
        }

        public void ToggleFilter(bool enableFilter)
        {
            ArmCam.GetComponent<postVHSPro>().enabled = enableFilter;
        }

        public void ToggleDoF(bool enableDoF)
        {
            if (enableDoF) _targetDoF = 0.6f;
            else _targetDoF = -1f;
        }

        public void ToggleDoFSetting(bool enableDoF)
        {
            _depthOfField.enabled.value = enableDoF;
        }

        private float CalculateDoFDistance()
        {
            Physics.Raycast(_cam.transform.position, _cam.forward, out var ray, 100.0f, DepthLayerMask);
            return ray.Equals(null) ? 100f : ray.distance;
        }
        
        public void TogglePsx(bool enablePsx)
        {
            _psxEffects.enabled = enablePsx;
        }
    
        #endregion
    }
}
