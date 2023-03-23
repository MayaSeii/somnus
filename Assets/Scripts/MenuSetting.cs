using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text _text;
    private Image _bg;
    
    // Start is called before the first frame update
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

    // Update is called once per frame
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
}
