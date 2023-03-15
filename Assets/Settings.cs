using System;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float MouseSensitivityX { get; set; }
    public static float MouseSensitivityY { get; set; }
    public static bool EnableVignette { get; set; }
    public static bool EnableHeadBobbing { get; set; }
    public static bool InvertY { get; set; }
    public static bool ToggleCrouch { get; set; }

    private void Awake()
    {
        MouseSensitivityX = 10f;
        MouseSensitivityY = 10f;
        EnableVignette = true;
        EnableHeadBobbing = true;
        InvertY = false;
        ToggleCrouch = false;
    }
}
