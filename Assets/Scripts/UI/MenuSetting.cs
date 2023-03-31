using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MenuSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TMP_Text _text;
        private Image _bg;
    
        private void Start()
        {
            _bg = GetComponentInChildren<Image>();
            _text = _bg.GetComponentInChildren<TMP_Text>();
        }
    
        private void OnDisable()
        {
            _text.color = Color.white;
            _bg.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = Color.blue;
            _bg.enabled = true;
        }
 
        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = Color.white;
            _bg.enabled = false;
        }
        
        public void PlaySound()
        {
            AudioManager.PlayOneShot(FMODEvents.Instance.SettingsButton, transform.position);
        }

        public void PlayTick()
        {
            AudioManager.PlayOneShot(FMODEvents.Instance.SliderTick, transform.position);
        }
    }
}
