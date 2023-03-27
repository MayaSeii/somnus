using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent (typeof(Button))]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TMP_Text _text;

        private void Start()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void OnDisable()
        {
            _text.color = Color.white;
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = Color.blue;
        }
 
        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = Color.white;
        }
    }
}
