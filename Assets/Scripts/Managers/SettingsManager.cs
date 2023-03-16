using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [field: SerializeField] public GameObject PauseMenu { get; set; }
        [field: SerializeField] public GameObject Crosshair { get; set; }
    
        private bool _isPaused;
        
        #region - Pause Menu -
        
        public void PauseGame(InputAction.CallbackContext context)
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
            Cursor.visible = _isPaused;
            Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            
            PauseMenu.SetActive(_isPaused);
            Crosshair.SetActive(!_isPaused);
        }
        
        #endregion
        
        #region - Mouse -

        public void ChangeSensitivityX(float value)
        {
            Settings.MouseSensitivityX = value;
        }
    
        public void ChangeSensitivityY(float value)
        {
            Settings.MouseSensitivityY = value;
        }

        public void ToggleInvertY(bool toggle)
        {
            Settings.InvertY = toggle;
        }
        
        public void ToggleInvertX(bool toggle)
        {
            Settings.InvertX = toggle;
        }
        
        #endregion

        #region - Movement -

        public void ToggleHeadBobbing(bool toggle)
        {
            Settings.EnableHeadBobbing = toggle;
        }

        public void ToggleCrouchToggle(bool toggle)
        {
            Settings.ToggleCrouch = toggle;
            ControlsManager.Instance.SetCrouchMode();
        }
        
        #endregion
        
        #region - Graphics -
        
        public void ToggleVignette(bool toggle)
        {
            Settings.EnableVignette = toggle;
        }

        public void ToggleMotionBlur(bool toggle)
        {
            UIManager.Instance.ToggleMotionBlur(toggle);
        }
        
        #endregion
    }
}
