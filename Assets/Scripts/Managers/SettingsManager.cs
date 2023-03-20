using UnityEngine;

namespace Managers
{
    public class SettingsManager : MonoBehaviour
    {
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

        public void ToggleFilter(bool toggle)
        {
            UIManager.Instance.ToggleFilter(toggle);
        }
        
        public void ToggleDoF(bool toggle)
        {
            UIManager.Instance.ToggleDoFSetting(toggle);
        }
        
        #endregion
    }
}
