using Audio;
using FMOD.Studio;
using Inputs;
using UI;
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
            if (Instance != null) Debug.LogError("Found more than one Settings Manager in the scene.");
            Instance = this;
        }

        #endregion

        #region - UNITY Start -
        
        private void Start()
        {
            CurrentPage = SettingsPage.Gameplay;
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

        public void ToggleChromaticAberration(bool toggle)
        {
            UIManager.Instance.ToggleChromaticAberration(toggle);
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
            
            PlayerPrefs.SetFloat($"volume{volumeType}", value);
        }
        
        public static void ToggleRandomMusic(bool toggle)
        {
            if (toggle) AudioManager.Instance.RandomMusicInstance.start();
            else AudioManager.Instance.RandomMusicInstance.stop(STOP_MODE.IMMEDIATE);
            
            PlayerPrefs.SetInt("randomMusic", toggle ? 1 : 0);
        }
        
        #endregion
    }
}
