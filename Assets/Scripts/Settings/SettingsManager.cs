using Inputs;
using UI;
using UnityEngine;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        #region - Mouse -

        public void ChangeSensitivityX(float value)
        {
            Config.MouseSensitivityX = value;
        }
    
        public void ChangeSensitivityY(float value)
        {
            Config.MouseSensitivityY = value;
        }

        public void ToggleInvertY(bool toggle)
        {
            Config.InvertY = toggle;
        }
        
        public void ToggleInvertX(bool toggle)
        {
            Config.InvertX = toggle;
        }
        
        #endregion

        #region - Movement -

        public void ToggleHeadBobbing(bool toggle)
        {
            Config.EnableHeadBobbing = toggle;
        }

        public void ToggleCrouchToggle(bool toggle)
        {
            Config.ToggleCrouch = toggle;
            ControlsManager.Instance.SetCrouchMode();
        }
        
        #endregion
        
        #region - Graphics -
        
        public void ToggleVignette(bool toggle)
        {
            Config.EnableVignette = toggle;
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
        
        public void TogglePsx(bool toggle)
        {
            UIManager.Instance.TogglePsx(toggle);
        }
        
        #endregion
    }
}
