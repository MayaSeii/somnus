using UnityEngine;

public class Settings : MonoBehaviour
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
}
