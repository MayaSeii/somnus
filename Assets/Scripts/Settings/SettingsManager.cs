using Audio;
using Inputs;
using UnityEngine;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }
        public SettingsPage CurrentPage { get; set; }
        
        #region - UNITY Awake -

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        #region - UNITY Start -
        
        private void Start()
        {
            CurrentPage = SettingsPage.Gameplay;
            Config.InitialiseConfigs();
        }
        
        #endregion

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
            Config.EnableMotionBlur = toggle;
        }

        public void ToggleFilter(bool toggle)
        {
            Config.EnableFilter = toggle;
        }
        
        public void ToggleDoF(bool toggle)
        {
            Config.EnableDoF = toggle;
        }
        
        public void TogglePsx(bool toggle)
        {
            Config.EnablePsx = toggle;
        }

        public void ToggleChromaticAberration(bool toggle)
        {
            Config.EnableChromaticAberration = toggle;
        }
        
        #endregion
        
        #region - Audio -

        public static void ChangeVolume(VolumeType volumeType, float value)
        {
            switch (volumeType)
            {
                default:
                case VolumeType.Master:
                    AudioManager.Instance.MasterVolume = value;
                    break;
                
                case VolumeType.Music:
                    AudioManager.Instance.MusicVolume = value;
                    break;
                
                case VolumeType.Ambience:
                    AudioManager.Instance.AmbienceVolume = value;
                    break;
                
                case VolumeType.Sound:
                    AudioManager.Instance.SoundVolume = value;
                    break;
                
                case VolumeType.Interface:
                    AudioManager.Instance.InterfaceVolume = value;
                    break;
            }
        }
        
        public static void ToggleRandomMusic(bool toggle)
        {
            Config.EnableRandomMusic = toggle;
        }
        
        #endregion
    }
}
