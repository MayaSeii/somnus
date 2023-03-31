using Audio;
using Coffee.UIExtensions;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Unmask _textMask;

    private void Awake()
    {
        _textMask = GetComponentInChildren<Unmask>();
    }

    private void Start()
    {
        _textMask.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.Hover);
        _textMask.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textMask.enabled = false;
    }
}
