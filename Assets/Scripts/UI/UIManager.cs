using System;
using System.Collections;
using System.Linq;
using Audio;
using Controllers;
using FMOD.Studio;
using General;
using Inputs;
using Nightmares;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        #region - VAR Interface Elements -
        
        [field: SerializeField, Header("Interface Elements")] public GameObject Crosshair { get; private set; }
        public GameObject Fader { get; private set; }
        
        #endregion
        
        #region - VAR Menus -
        
        [field: SerializeField, Header("Menus")] public GameObject PauseMenu { get; private set; }
        [field: SerializeField] public GameObject SettingsMenu { get; private set; }
        [field: SerializeField] public GameObject CpnfirmationWindow { get; private set; }

        private bool _isPaused;
        
        #endregion
        
        #region - VAR Settings -
        
        [field: Header("Settings")]
        [field: SerializeField] public PageButton[] Buttons { get; private set; }
        [field: SerializeField] public GameObject[] Pages { get; private set; }
        
        #endregion
        
        #region - VAR Blinking & Rest -
    
        [field: SerializeField, Header("Blinking")] public Image BlinkOverlay { get; private set; }
        [field: SerializeField] public Slider RestMeter { get; private set; }
        [field: SerializeField] public Slider ArmRestMeter { get; private set; }

        private float _targetBlinkOpacity;
    
        #endregion
    
        #region - VAR Post-Processing -
    
        [field: SerializeField, Header("Post-Processing")] public Transform PostProcessCamera { get; private set; }
        [field: SerializeField] public LayerMask DepthLayerMask { get; private set; }
        
        private Transform _cam;
        
        private PostProcessVolume _postProcessVolume;
        private Vignette _vignette;
        private MotionBlur _motionBlur;
        private ChromaticAberration _chromaticAberration;
        private DepthOfField _depthOfField;
        private PSXEffects _psxEffects;
    
        private float _targetVignette;
        private float _targetDoF;
        private float _dynamicDoFDistance;
    
        #endregion

        #region - UNITY Awake -
    
        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one UI Manager in the scene.");
            Instance = this;
            
            Fader = GameObject.FindWithTag("Fader");
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
            if (_postProcessVolume != null) _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            _psxEffects = PostProcessCamera.GetComponent<PSXEffects>();
            
            _targetVignette = 0.27f;
            _targetDoF = -1f;

            var cursorTexture = Resources.Load<Texture2D>("Sprites/Cursor");
            Cursor.SetCursor(cursorTexture, new Vector2(8, 0), CursorMode.Auto);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

            ControlsManager.Instance.TogglePlayerInputMap(!_isPaused);
            GameManager.Instance.GameplayUICamera.gameObject.SetActive(!_isPaused);
            
            PauseMenu.SetActive(_isPaused);
            Crosshair.SetActive(!_isPaused);
            
            if (_isPaused) AudioManager.Instance.EventInstances["MenuBuzz"].start();
            else
            {
                AudioManager.Instance.EventInstances["MenuBuzz"].stop(STOP_MODE.IMMEDIATE);
                AudioManager.PlayOneShot(FMODEvents.Instance.MenuExit, transform.position);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        #endregion
        
        #region - Settings Menu -
        
        public void ToggleSettings()
        {
            if (SettingsMenu.activeInHierarchy) AudioManager.PlayOneShot(FMODEvents.Instance.SettingsButton, transform.position);
            
            PauseMenu.SetActive(SettingsMenu.activeInHierarchy);
            SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
        }

        public void TogglePage(int index)
        {
            SettingsManager.Instance.CurrentPage = (SettingsPage) index;
            Pages.ToList().ForEach(p => p.SetActive(Array.IndexOf(Pages, p) == index));
            Buttons.ToList().ForEach(b => b.GetComponentInChildren<TMP_Text>().color = Array.IndexOf(Buttons, b) == index ? Color.blue : Color.white);
        }

        public PageButton ActiveButton()
        {
            return Buttons[(int) SettingsManager.Instance.CurrentPage];
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
            ArmRestMeter.value = Mathf.Clamp(value, ArmRestMeter.minValue, ArmRestMeter.maxValue);

            if (!(RestMeter.value >= RestMeter.maxValue)) return;
            
            GameManager.Instance.Player.Stop();
            foreach (var n in FindObjectsOfType<NightmareController>()) Destroy(n);
            
            StartCoroutine(FadeOut());
        }
        
        public static IEnumerator FadeOut()
        {
            PlayerController.Instance.Stop();
            yield return new WaitForSeconds(3f);
            Instance.Fader.SetActive(true);
            Instance.Fader.GetComponent<Fader>().TargetAlpha = 1f;
            
            yield return new WaitForSeconds(6f);
            GameManager.ReturnToMenu();
        }
    
        #endregion

        #region - Post-Processing -
    
        public void ToggleCrouchVignette(bool enableVignette)
        {
            if (enableVignette) _targetVignette = Config.EnableVignette ? 0.5f : 0.27f;
            else _targetVignette = 0.27f;
        }

        public void ToggleMotionBlur(bool enableMotionBlur)
        {
            _motionBlur.enabled.value = enableMotionBlur;
        }

        public void ToggleFilter(bool enableFilter)
        {
            FindObjectOfType<postVHSPro>().enabled = enableFilter;
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
            //_psxEffects.enabled = enablePsx;
            return;
        }

        public void ToggleChromaticAberration(bool enableChromaticAberration)
        {
            _chromaticAberration.enabled.value = enableChromaticAberration;
        }
    
        #endregion
    }
}
