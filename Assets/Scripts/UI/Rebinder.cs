using System.Linq;
using Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebinder : MonoBehaviour
{
    [field: SerializeField] public InputType InputType { get; private set; }
    [field: SerializeField] public bool IsComposite { get; private set; }
    [field: SerializeField] public int CompositeDirection { get; private set; }

    private TMP_Text _text;
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    private InputAction _inputAction;
    private int _bindingIndex;

    private void Awake()
    {
        _text = GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        _inputAction = ControlsManager.Instance.GetInputReference(InputType);
        _bindingIndex = IsComposite ? CompositeDirection : _inputAction.GetBindingIndexForControl(_inputAction.controls[0]);

        UpdateBindingDisplay();
    }

    public void StartRebinding()
    {
        _text.text = "Press Key";
        
        _inputAction.Disable();

        if (IsComposite)
        {
            _rebindingOperation = _inputAction.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape")
                .WithTargetBinding(CompositeDirection)
                .OnMatchWaitForAnother(.1f)
                .OnCancel(_ => RebindCancelled())
                .OnComplete(_ => RebindComplete())
                .Start();
        }
        else
        {
            _rebindingOperation = _inputAction.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(.1f)
                .OnCancel(_ => RebindCancelled())
                .OnComplete(_ => RebindComplete())
                .Start();
        }
    }

    private void RebindComplete()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        if (CheckDuplicateBindings(_inputAction, _bindingIndex))
        {
            _inputAction.RemoveBindingOverride(_bindingIndex);
            CleanUp();
            StartRebinding();
            return;
        }
        
        UpdateBindingDisplay();
        CleanUp();
        RefreshInput();
        Save();
    }
    
    private void RebindCancelled()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        UpdateBindingDisplay();
        CleanUp();
        RefreshInput();
    }

    public void UpdateBindingDisplay()
    {
        _text.text = _inputAction.GetBindingDisplayString(_bindingIndex);
    }

    private void CleanUp()
    {
        _rebindingOperation.Dispose();
        _rebindingOperation = null;
    }

    private void RefreshInput()
    {
        _inputAction.Enable();
        _inputAction.actionMap.Disable();
    }

    private static bool CheckDuplicateBindings(InputAction action, int index)
    {
        var newBinding = action.bindings[index];
        return action.actionMap.bindings.Where(binding => binding.id != newBinding.id).Any(binding => binding.effectivePath == newBinding.effectivePath);
    }

    public void Save()
    {
        var rebinds = _inputAction.actionMap.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
