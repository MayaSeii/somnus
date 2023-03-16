using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        #region - VAR Blinking & Rest -
    
        [field: SerializeField, Header("Blinking")] public Image BlinkOverlay { get; set; }
        [field: SerializeField] public Slider RestMeter { get; set; }

        private float _targetBlinkOpacity;
    
        #endregion
    
        #region - VAR Post-Processing -
    
        private PostProcessVolume _postProcessVolume;
        private Vignette _vignette;
        private MotionBlur _motionBlur;
        private Camera _cam;
    
        private float _targetVignette;
    
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
            _cam = Camera.main;
            _postProcessVolume = _cam != null ? _cam.GetComponent<PostProcessVolume>() : null;
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _vignette);
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _motionBlur);
            _targetVignette = 0.27f;
        
            Cursor.visible = false;
        }
    
        #endregion

        #region - UNITY Update -
    
        private void Update()
        {
            BlinkOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(BlinkOverlay.color.a, _targetBlinkOpacity, 20f * Time.deltaTime));
            _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _targetVignette, 15f * Time.deltaTime);
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
    
        #endregion
    }
}
