using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PageButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        private TMP_Text _text;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            var isActive = UIManager.Instance.ActiveButton() == this;

            var colours = _button.colors;
            colours.normalColor = isActive == this ? Color.white : Color.clear;
            _button.colors = colours;

            if (isActive) _text.color = Color.blue;
        }

        private void OnDisable()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            _text.color = Color.white;
        }

        public void ChangeSettingsPage()
        {
            AudioManager.PlayOneShot(FMODEvents.Instance.PageButton, transform.position);
            UIManager.Instance.TogglePage(Array.IndexOf(UIManager.Instance.Buttons, this));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = Color.blue;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            _text.color = Color.white;
        }
    }
}