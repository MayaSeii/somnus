using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    [field: SerializeField] public GameObject PauseMenu { get; set; }
    [field: SerializeField] public GameObject Crosshair { get; set; }
    
    private bool _isPaused;

    public void ChangeSensitivityX(float value)
    {
        Settings.MouseSensitivityX = value;
    }
    
    public void ChangeSensitivityY(float value)
    {
        Settings.MouseSensitivityY = value;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        PauseMenu.SetActive(_isPaused);
        Crosshair.SetActive(!_isPaused);
        Cursor.visible = _isPaused;
    }

    public void ToggleVignette(bool toggle)
    {
        Settings.EnableVignette = toggle;
    }

    public void ToggleHeadBobbing(bool toggle)
    {
        Settings.EnableHeadBobbing = toggle;
    }

    public void ToggleInvertY(bool toggle)
    {
        Settings.InvertY = toggle;
    }

    public void ToggleCrouchToggle(bool toggle)
    {
        Settings.ToggleCrouch = toggle;
        ControlsManager.Instance.ToggleCrouchToggle();
    }
}
