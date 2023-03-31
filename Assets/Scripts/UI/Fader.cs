using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = true;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        
        var colour = _image.color;

        if (colour.a <= 0) gameObject.SetActive(false);
        
        colour.a -= Time.deltaTime;
        _image.color = colour;
    }
}
