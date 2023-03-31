using Audio;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Settings
{
    public class Config : MonoBehaviour
    {
        #region - Mouse -
    
        public static float MouseSensitivityX { get; set; }
        public static float MouseSensitivityY { get; set; }
        public static bool InvertX { get; set; }
        public static bool InvertY { get; set; }
    
        #endregion
    
        #region - Movement -
    
        public static bool EnableHeadBobbing { get; set; }
        public static bool ToggleCrouch { get; set; }
    
        #endregion
    
        #region - Post-Processing -
    
        public static bool EnableVignette { get; set; }

        private static bool _enableMotionBlur;
        public static bool EnableMotionBlur
        {
            get => _enableMotionBlur;
            set
            {
                var ppv = FindObjectOfType<PostProcessVolume>();
                if (ppv)
                {
                    ppv.profile.TryGetSettings(out MotionBlur motionBlur);
                    motionBlur.enabled.value = value;
                }
                
                _enableMotionBlur = value;
            }
        }

        private static bool _enableFilter;
        public static bool EnableFilter
        {
            get => _enableFilter;
            set
            {
                var filter = FindObjectOfType<postVHSPro>();
                if (filter) filter.enabled = value;
                
                _enableFilter = value;
            }
        }

        private static bool _enablePsx;
        public static bool EnablePsx
        {
            get => _enablePsx;
            set
            {
                var filter = FindObjectOfType<PSXEffects>();
                if (filter) filter.enabled = value;
                
                _enablePsx = value;
            }
        }

        private static bool _enableDoF;
        public static bool EnableDoF
        {
            get => _enableDoF;
            set
            {
                var ppv = FindObjectOfType<PostProcessVolume>();
                if (ppv)
                {
                    ppv.profile.TryGetSettings(out DepthOfField depthOfField);
                    depthOfField.enabled.value = value;
                }
                
                _enableDoF = value;
            }
        }

        private static bool _enableChromaticAberration;
        public static bool EnableChromaticAberration
        {
            get => _enableChromaticAberration;
            set
            { 
                var ppv = FindObjectOfType<PostProcessVolume>();
                if (ppv)
                {
                    ppv.profile.TryGetSettings(out ChromaticAberration chromaticAberration);
                    chromaticAberration.enabled.value = value;
                }
                
                _enableChromaticAberration = value;
            }
        }

        #endregion
        
        #region - Audio -
        
        private static bool _enableRandomMusic;
        public static bool EnableRandomMusic
        {
            get => _enableRandomMusic;
            set
            {
                if (AudioManager.Instance.EventInstances.ContainsKey("RandomMusic"))
                {
                    if (value) AudioManager.Instance.EventInstances["RandomMusic"].start();
                    else AudioManager.Instance.EventInstances["RandomMusic"].stop(STOP_MODE.IMMEDIATE);
                }
                
                _enableRandomMusic = value;
            }
        }
        
        #endregion
    
        #region - UNITY Awake -
    
        private void Awake()
        {
            MouseSensitivityX = 10f;
            MouseSensitivityY = 10f;
        
            InvertX = false;
            InvertY = false;
        
            EnableHeadBobbing = true;
            ToggleCrouch = false;
        
            EnableVignette = true;
        }
    
        #endregion
        
        #region - Reset -

        public static void InitialiseConfigs()
        {
            EnableFilter = true;
            EnablePsx = true;
            EnableChromaticAberration = true;
            EnableDoF = true;
            EnableMotionBlur = true;
            EnableRandomMusic = true;
        }

        public static void ResetConfigs()
        {
            EnableFilter = EnableFilter;
            EnablePsx = EnablePsx;
            EnableChromaticAberration = EnableChromaticAberration;
            EnableDoF = EnableDoF;
            EnableMotionBlur = EnableMotionBlur;
            EnableRandomMusic = EnableRandomMusic;
        }
        
        #endregion
    }
}
