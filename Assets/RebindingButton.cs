using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RebindingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private Sprite _activeSprite;
    private Sprite _inactiveSprite;

    private bool _hover;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _activeSprite = Resources.Load<Sprite>("Sprites/CheckboxSelected");
        _inactiveSprite = Resources.Load<Sprite>("Sprites/Checkbox");
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

    private void OnDisable()
    {
        _button.image.sprite = _inactiveSprite;
        _hover = false;
    }
}
