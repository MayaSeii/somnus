using System;
using Audio;
using Coffee.UIExtensions;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class TitlePageButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        private Unmask _textMask;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _textMask = GetComponentInChildren<Unmask>();
        }

        private void Update()
        {
            var isActive = TitleUIManager.Instance.ActiveButton() == this;

            var colours = _button.colors;
            colours.normalColor = isActive == this ? Color.white : Color.clear;
            _button.colors = colours;

            if (isActive) _textMask.enabled = true;
        }

        private void OnDisable()
        {
            if (!EventSystem.current) return;
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            _textMask.enabled = false;
        }

        public void ChangeSettingsPage()
        {
            RuntimeManager.PlayOneShot(FMODEvents.Instance.PageButton);
            TitleUIManager.Instance.TogglePage(Array.IndexOf(TitleUIManager.Instance.Buttons, this));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RuntimeManager.PlayOneShot(FMODEvents.Instance.Hover);
            _textMask.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            _textMask.enabled = false;
        }
    }
}