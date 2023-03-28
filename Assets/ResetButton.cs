using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetButton : MonoBehaviour
{
    private InputAction _action;
    private Rebinder _rebinder;

    private void Awake()
    {
        _rebinder = transform.parent.GetComponent<Rebinder>();
    }

    private void Start()
    {
        _action = ControlsManager.Instance.GetInputReference(_rebinder.InputType);
    }

    public void ResetInputBinding()
    {
        _action.RemoveAllBindingOverrides();
        _rebinder.UpdateBindingDisplay();
        _rebinder.Save();
    }
}
