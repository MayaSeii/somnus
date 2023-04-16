using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public float TargetAlpha { get; set; }
    private Image _image;

    private void Awake()
    {
        TargetAlpha = 0f;
        _image = GetComponent<Image>();
        _image.enabled = true;
    }

    private void Update()
    {
        var colour = _image.color;
        var currentOpacity = colour.a;

        if (currentOpacity > TargetAlpha)
        {
            colour.a = Mathf.Max(TargetAlpha, colour.a - Time.deltaTime);
        }
        else if (currentOpacity < TargetAlpha)
        {
            colour.a = Mathf.Min(TargetAlpha, colour.a + Time.deltaTime);
        }
        
        _image.color = colour;
        
        switch (colour.a)
        {
            case 0 when gameObject.activeInHierarchy:
                gameObject.SetActive(false);
                break;
            
            case > 0 when !gameObject.activeInHierarchy:
                gameObject.SetActive(true);
                break;
        }
    }
}
