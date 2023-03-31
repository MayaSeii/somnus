using Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private Sprite _activeSprite;
    private Sprite _inactiveSprite;
    private InputAction _action;
    private Rebinder _rebinder;

    private bool _hover;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _activeSprite = Resources.Load<Sprite>("Sprites/CheckboxSelected");
        _inactiveSprite = Resources.Load<Sprite>("Sprites/Checkbox");
        
        _rebinder = transform.parent.GetComponent<Rebinder>();
    }

    private void Start()
    {
        _action = ControlsManager.Instance.GetInputReference(_rebinder.InputType);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == _button.gameObject && _button.image.sprite != _activeSprite)
        {
            _button.image.sprite = _activeSprite;
        }
        else if (!_hover && EventSystem.current.currentSelectedGameObject != _button.gameObject && _button.image.sprite != _inactiveSprite)
        {
            _button.image.sprite = _inactiveSprite;
        }
    }

    private void OnDisable()
    {
        _button.image.sprite = _inactiveSprite;
        _hover = false;
    }

    public void ResetInputBinding()
    {
        _action.RemoveAllBindingOverrides();
        _rebinder.UpdateBindingDisplay();
        _rebinder.Save();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.image.sprite = _activeSprite;
        _hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _button.image.sprite = _inactiveSprite;
        _hover = false;
    }
}
